using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 movement;
    Animator controller;

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

    //Stats attack, attack speed


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
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

        this.transform.position += new Vector3(movement.x, movement.y, 0) * speed / 100;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
