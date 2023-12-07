using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

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
        Parry,
        Dialogue
    }
    public states currentState = states.Base;

    public float speed = 0.25f;
    public float speedUpgrade = 1f;
    public float dashPower = 20f;
    float kbForce = 2f;

    bool stun = false;
    float stunTimer = 0.25f;

    public bool dashOnMovement = false; //modif pour faire je usans soruis avec autoaim (+ mannette)

   /*public bool mouseLock = false;
    Vector2 mouseLastPos = Vector2.zero;*/

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
        }

        if(stun)
        {
            stunTimer -= Time.deltaTime;
            if(stunTimer <= 0) 
            {
                stun = false;
                stunTimer = 0.25f;
            }
            else
            {
                return;
            }
        }

        if (pause) return;

        if (Input.GetAxis("Slash") != 0)
        {
            if(currentState == states.Base)
                Attack();
        }
        if (Input.GetAxis("Parry") != 0)
        {
            if(currentState == states.Base)
                Parry();
        }
        if (Input.GetAxis("Dodge") != 0)
        {
            if(currentState == states.Base)
                Dodge();
        }

        /*if (Input.GetKeyDown(KeyCode.E))
        {
            mouseLock = true;
            Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(this.transform.position));
            mouseLastPos = Camera.main.WorldToScreenPoint(this.transform.position);
        }*/

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            if(dashOnMovement)
                dashOnMovement = false;
            else
                dashOnMovement = true;
            Debug.Log(dashOnMovement);
        }*/

        /*if(mouseLock)
        {
            Vector2 mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0);
            //Vector2 dist = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, this.transform.position.z);
            Debug.Log(Vector2.Distance(Input.mousePosition, new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0)));
            
            if (Vector2.Distance(Input.mousePosition, new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0)) > 150) 
            {
                Mouse.current.WarpCursorPosition(mouseLastPos);
            }
            else
                mouseLastPos = Input.mousePosition;
        }*/

        if (IFrameFlag)
        {
            timer -= Time.deltaTime;
            if(currentState == states.Parry) 
            {
                IFrameFlag = false;
                IFrame = true;
                UIManager.Instance.dps.Light("P",true);
                parriedMinion.GetComponent<Minion>().Stunned();
                VFXManager.Instance.PlayEffectOn("Parry", this.gameObject);
                VFXManager.Instance.HitStop();
                AddScore();
            }
            if(timer <= 0)
            {
                IFrameFlag = false;
                IFrame = false;
                timer = IFrameCD;
                TakeDamage();
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
        if (stun) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        body.velocity *= 0.9f;
        /*if(mouseLock)
            Mouse.current.WarpCursorPosition(mouseLastPos + new Vector2(Camera.main.WorldToScreenPoint(movement * (speed * speedUpgrade)).x,Camera.main.WorldToScreenPoint(movement * (speed * speedUpgrade)).y));*/
        this.transform.position += new Vector3(movement.x, movement.y, 0).normalized * (speed * speedUpgrade);
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
        VFXManager.Instance.PlayEffectOn("DodgeTrail",this.gameObject);

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
        VFXManager.Instance.HitStop();
        stun = true;
        body.velocity = Vector2.zero;
        controller.Play("HitStun");
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
                {
                    AddScore();
                    VFXManager.Instance.HitStop();
                    UIManager.Instance.dps.Light("D",true);
                    if (this.gameObject.GetComponentInChildren<WeaponParent>().faceR)
                        VFXManager.Instance.PlayEffectAt("Dodge", this.transform, true);
                    else
                        VFXManager.Instance.PlayEffectAt("Dodge", this.transform);
                }
                collision.gameObject.GetComponent<MageProjectile>().hitFlag = false;
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
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
                UIManager.Instance.dps.Light("P",true);
                collision.gameObject.GetComponent<Minion>().Stunned();
                AddScore();
                VFXManager.Instance.HitStop();
                VFXManager.Instance.PlayEffectOn("Parry",this.gameObject);
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
