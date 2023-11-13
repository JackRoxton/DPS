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
    public float dashPower = 10f;
    
    // public float attacPower;
    //public float attackSpeed?

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
            Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Parry();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        GameManager.Instance.AddScore(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        if(collision.gameObject.GetComponent<MageProjectile>() != null ) {
            if (currentState != states.Dodge) TakeDamage();
            else AddScore();//destroy projectile
        }
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (currentState != states.Parry && !weapon.hitboxActive) TakeDamage();
            else AddScore();//destroy or stun minon  //check tag, s�parer dans minion ?
        }

        /*var collide = collision.gameObject.GetComponent<Component>();
        switch(collide)
        {
            case MageProjectile mageprojectile:
                if (currentState != states.Dodge) TakeDamage();
                else AddScore();
                break;

            case Minion minion:
                if (currentState != states.Parry) TakeDamage();
                else AddScore();
                break;
        }*/
    }
}
