using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator controller;
    public bool hitboxActive = false;
    public Player player;
    bool attackFlag = false;
    bool hitflag = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(attackFlag)
            if(!hitboxActive)
                attackFlag = false;*/
    }

    public void Attack()
    {
        if(attackFlag) return; 
        controller.Play("Attack", 0);
        hitflag = false;
        attackFlag = true;
        StartCoroutine(_Attack());
    }

    public IEnumerator _Attack()
    {
        yield return new WaitForSeconds(0.25f);
        attackFlag = false;
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
                Transform trs = this.transform;
                trs.position = Physics2D.ClosestPoint(this.transform.position,collision);
                if (!UIManager.Instance.dps.GetState("S"))
                    VFXManager.Instance.PlayEffectAt("Success", trs);
                UIManager.Instance.dps.Light("S",true);
                VFXManager.Instance.PlayEffectAt("Hit",trs);
                SoundManager.Instance.Play("hit");
                VFXManager.Instance.HitStop();
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
            if (hitboxActive && !hitflag)
            {
                collision.gameObject.GetComponent<Mage>().TakeDamage();
                Transform trs = this.transform;
                trs.position = Physics2D.ClosestPoint(this.transform.position, collision);
                if (!UIManager.Instance.dps.GetState("S"))
                    VFXManager.Instance.PlayEffectAt("Success", trs);
                UIManager.Instance.dps.Light("S",true);
                player.AtkKnockBack(player.transform.position - collision.gameObject.transform.position);
                VFXManager.Instance.PlayEffectAt("Hit", trs);
                SoundManager.Instance.Play("hit");
                hitflag = true;
                VFXManager.Instance.HitStop();
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
