using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator controller;
    public bool hitboxActive = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        controller.Play("Attack", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Mage>() != null)
        {
            if (hitboxActive) collision.gameObject.GetComponent<Mage>().TakeDamage();
        }
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (hitboxActive) collision.gameObject.GetComponent<Minion>().TakeDamage();
            else Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
        }
        else if (collision.gameObject.GetComponent<MageProjectile>() != null)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, false);
        }
    }

}
