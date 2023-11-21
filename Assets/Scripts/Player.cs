using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 movement;
    Animator controller;
    Rigidbody2D body;
    public Weapon weapon;

    public bool endFlag = false;
    bool pause = false;

    public enum states
    {
        Base,
        Dodge,
        Parry
    }
    public states currentState = states.Base;

    public float speed = 0.15f;
    public float speedUpgrade = 1f;
    public float dashPower = 5f;
    float kbForce = 2f;

    public bool dashOnMovement = false;

    bool IFrame = true;
    bool IFrameFlag = false;
    float IFrameCD = 1f;
    float IFrameTimer = 0.1f;
    float timer;
    GameObject parriedMinion;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (endFlag) return;
        if (pause) return;

        if (Input.GetMouseButtonDown(0))
        {
            if(currentState == states.Base)
                Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(currentState == states.Base)
                Parry();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentState == states.Base)
                Dodge();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(dashOnMovement)
                dashOnMovement = false;
            else
                dashOnMovement = true;
            Debug.Log(dashOnMovement);
        }

        if(IFrameFlag)
        {
            timer -= Time.deltaTime;
            if(currentState == states.Parry) 
            {
                IFrameFlag = false;
                IFrame = true;
                UIManager.Instance.dps.PLight(true);
                parriedMinion.GetComponent<Minion>().Stunned();
                AddScore();
            }
            if(timer <= 0)
            {
                IFrameFlag = false;
                IFrame = false;
                timer = IFrameCD;
            }
        }
        if (!IFrame)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                IFrame = true;
            }
        }


        //movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (endFlag) return;
        if (pause) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        body.velocity *= 0.9f;
        this.transform.position += new Vector3(movement.x, movement.y, 0) * (speed * speedUpgrade);
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
    }

    private void Attack()
    {
        weapon.Attack();
    }

    private void Parry()
    {
        controller.Play("Parry", 0);
        SoundManager.Instance.Play("parry");
    }

    private void Dodge()
    {
        controller.Play("Dodge", 0);
        SoundManager.Instance.Play("dodge");

        Vector3 dir = Vector3.zero;

        if (dashOnMovement)
        {
            dir = movement;
        }
        else
        {
            Vector2 mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0);
            dir = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, this.transform.position.z);
        }

        dir.Normalize();
        body.velocity = new Vector2(dir.x * dashPower,dir.y * dashPower);
    }

    private void TakeDamage()
    {
        GameManager.Instance.TakeDamage();
        SoundManager.Instance.Play("phit");
    }

    private void AddScore()
    {
        GameManager.Instance.AddScore();
    }

    public void KnockBack(Vector2 kb)
    {
        body.velocity += kb.normalized * kbForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        if(collision.gameObject.GetComponent<MageProjectile>() != null ) {
            if (currentState != states.Dodge)
            {
                TakeDamage();
                collision.gameObject.GetComponent<MageProjectile>().Die();
            }
            else 
            {
                if (collision.gameObject.GetComponent<MageProjectile>().hitFlag == true)
                    AddScore();
                collision.gameObject.GetComponent<MageProjectile>().hitFlag = false;
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                UIManager.Instance.dps.DLight(true);
            }
        }
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (currentState != states.Parry && !weapon.hitboxActive && collision.gameObject.GetComponent<Minion>().hitboxActive && !collision.gameObject.GetComponent<Minion>().stun && collision.gameObject.GetComponent<Minion>().dmgFlag)
            {
                if (IFrame)
                {
                    if(!IFrameFlag)
                        timer = IFrameTimer;
                    IFrameFlag = true;
                    parriedMinion = collision.gameObject;
                }
                else
                {
                    TakeDamage();
                    collision.gameObject.GetComponent<Minion>().dmgFlag = false;
                }
            }
            else if (currentState == states.Parry && collision.gameObject.GetComponent<Minion>().hitboxActive && !collision.gameObject.GetComponent<Minion>().stun && !collision.isTrigger)
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                UIManager.Instance.dps.PLight(true);
                collision.gameObject.GetComponent<Minion>().Stunned();
                AddScore();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Minion>() != null)
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, false);
    }

}
