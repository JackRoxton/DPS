using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public Animator controller;
    public bool hitboxActive = false;
    public Player player;
    public WeaponParent parent;
    bool attackFlag = false;
    bool hitflag = false;
    public GameObject slash;
    GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = Vector3.zero;
    }

    public void Attack()
    {
        if(attackFlag) return;
        SoundManager.Instance.Play("slash");
        go = Instantiate(slash, transform.position, Quaternion.identity);
        go.transform.rotation = this.transform.rotation;
        go.transform.localScale = parent.transform.localScale *1.25f;
        go.GetComponentInChildren<VisualEffect>().Play();
        controller.Play("Attack", 0);
        hitflag = false;
        attackFlag = true;
        StartCoroutine(_Attack());
    }

    public IEnumerator _Attack()
    {
        yield return new WaitForSeconds(0.18f/controller.speed);
        attackFlag = false;
        Destroy(go);
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
        if (collision.gameObject.GetComponent<Minion>() != null)
        {
            if (hitboxActive && collision.tag == "Minion") 
            {
                collision.gameObject.GetComponent<Minion>().TakeDamage();
                Transform trs = this.transform;
                trs.position = Physics2D.ClosestPoint(this.transform.position,collision);
                if (!UIManager.Instance.dps.GetState("S"))
                {
                    VFXManager.Instance.PlayEffectAt("Success", trs);
                    UIManager.Instance.FloatingText(this.transform.position, "slash", false, null, new Color32(0xea,0x34,0x34,255));//#ea3434
                }
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

        if (collision.tag == "Spikes")
        {
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), collision);
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
