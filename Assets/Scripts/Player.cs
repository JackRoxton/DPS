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

    public enum states
    {
        Base,
        Dodge,
        Parry
    }
    public states currentState = states.Base;

    [Range(0f, 10f)]
    public float speed = 1f;

    [Range(0f, 50f)]
    public float dashPower = 20f;

    float kbForce = 2f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0) // voir meilleurs inputs
        {
            movement.x = Input.GetAxis("Horizontal");
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            movement.y = Input.GetAxis("Vertical");
        }

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

        body.velocity *= 0.99f;
        this.transform.position += new Vector3(movement.x, movement.y, 0) * speed / 100;
        movement = Vector2.zero;
    }

    private void Attack()
    {
        weapon.Attack();
    }

    private void Parry()
    {
        controller.Play("Parry", 0);
    }

    private void Dodge()
    {
        controller.Play("Dodge", 0);

        Vector2 mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0);
        Vector3 dir = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, this.transform.position.z);
        dir.Normalize();
        body.velocity = new Vector2(dir.x * dashPower,dir.y * dashPower);
    }

    private void TakeDamage()
    {
        GameManager.Instance.TakeDamage();
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
            if (currentState != states.Dodge) TakeDamage();
            else
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(),collision,true);
                AddScore();
                UIManager.Instance.dps.DLight(true);
            }
        }
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (currentState != states.Parry && !weapon.hitboxActive && collision.gameObject.GetComponent<Minion>().hitboxActive && !collision.gameObject.GetComponent<Minion>().stun)
            {
                TakeDamage();
                collision.gameObject.GetComponent<Minion>().hitboxActive = false;
            }
            else if (currentState == states.Parry && collision.gameObject.GetComponent<Minion>().hitboxActive && !collision.gameObject.GetComponent<Minion>().stun)
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
