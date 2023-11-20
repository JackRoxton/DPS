using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator controller;
    public bool hitboxActive = false;
    public Player player;
    bool attackFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackFlag)
            if(!hitboxActive)
                attackFlag = false;
    }

    public void Attack()
    {
        controller.Play("Attack", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        /*if (collision.gameObject.GetComponent<Mage>() != null)
        {
            if (hitboxActive)
            {
                collision.gameObject.GetComponent<Mage>().TakeDamage();
                UIManager.Instance.dps.SLight(true);
                player.KnockBack(player.transform.position - collision.gameObject.transform.position);
            }
        }*/
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (hitboxActive && !collision.isTrigger) 
            {
                collision.gameObject.GetComponent<Minion>().TakeDamage();
                UIManager.Instance.dps.SLight(true);
                SoundManager.Instance.Play("hit");
            }
            else Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
        }
        else if (collision.gameObject.GetComponent<MageProjectile>() != null)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Mage>() != null)
        {
            if (hitboxActive && !attackFlag)
            {
                collision.gameObject.GetComponent<Mage>().TakeDamage();
                UIManager.Instance.dps.SLight(true);
                player.KnockBack(player.transform.position - collision.gameObject.transform.position);
                attackFlag = true;
                SoundManager.Instance.Play("hit");
            }
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
