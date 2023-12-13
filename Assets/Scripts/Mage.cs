using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mage : MonoBehaviour
{
    public GameObject Player;
    public GameObject ProjectilePrefab;
    GameObject CurrentProjectile;
    public GameObject AttackPrefab;
    GameObject CurrentAttack;
    public float projTime = 5f;
    [NonSerialized] public float ptimer;
    public float atkTime = 6f;
    [NonSerialized] public float atimer;
    public float spellSpeed;

    public bool spell = false;

    public bool projCast = false;
    public bool atkCast = false;
    public bool tuto = false;
    public bool pause = false;

    public float phaseMult = 1;

    Animator controller;

    //bool faceR;
    // Start is called before the first frame update
    void Start()
    { 
        projTime -= phaseMult;
        atkTime -= phaseMult;
        ptimer = projTime;
        atimer = atkTime;
        spellSpeed = 5f;
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tuto) return;
        if(pause) return;
        ptimer -= Time.deltaTime;

        /*if (Player.transform.position.x < this.transform.position.x)
            faceR = true;
        else
            faceR = false;*/

        if (ptimer <= 0)
        {
            StartCoroutine(CastProj());
            ptimer = projTime + Random.Range(0, 2);
        }
        
        if (Vector3.Distance(Player.transform.position, this.transform.position) <= 4f)
        {
            atimer -= Time.deltaTime;
            if (atimer <= 0)
            {
                StartCoroutine(CastAtk());
                atimer = atkTime + Random.Range(0, 2);
            }
        }
        
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
    }

    private void OnDestroy()
    {
        if(CurrentProjectile != null)
            Destroy(CurrentProjectile.gameObject);
        if(CurrentAttack != null)
            Destroy(CurrentAttack.gameObject);
    }

    public IEnumerator CastProj()
    {
        while (spell) yield return new WaitForEndOfFrame();
        spell = true;
        SoundManager.Instance.Play("spell");
        VFXManager.Instance.PlayEffectOn("Circle", this.gameObject);
        controller.Play("SpellCast",0);
        yield return new WaitUntil(ProjCast);
        CurrentProjectile = Instantiate(ProjectilePrefab, this.transform.position,Quaternion.identity);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(),CurrentProjectile.GetComponent<Collider2D>());
        CurrentProjectile.GetComponent<MageProjectile>().body.velocity = (new Vector2(Player.transform.position.x - this.transform.position.x, Player.transform.position.y -this.transform.position.y).normalized)*spellSpeed;
        projCast = false;
        spell = false;
        yield return null;
    }

    public IEnumerator CastAtk()
    {
        while(spell) yield return new WaitForEndOfFrame();
        spell = true;
        SoundManager.Instance.Play("attack");
        VFXManager.Instance.PlayEffectOn("Circle", this.gameObject);
        controller.Play("AtkCast", 0);
        yield return new WaitUntil(AtkCast);
        CurrentAttack = Instantiate(AttackPrefab, this.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), CurrentAttack.GetComponent<Collider2D>());
        atkCast = false;
        spell = false;
        yield return null;
    }

    public void Teleport(Transform spot)
    {
        //effet
        this.transform.position = spot.position;
    }

    public void TakeDamage()
    {
        GameManager.Instance.ScreenShake();
        GameManager.Instance.AddScore();
    }

    public bool ProjCast()
    {
        return projCast;
    }
    public bool AtkCast()
    {
        return atkCast;
    }
}
