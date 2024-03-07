using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mage : MonoBehaviour
{
    public GameObject Player;
    public GameObject ProjectilePrefab;
    public GameObject AttackPrefab;
    public GameObject LaserPrefab;
    List<GameObject> CurrentSpells;
    public float projTime = 4f;
    [NonSerialized] public float ptimer;
    public float atkTime = 5f;
    [NonSerialized] public float atimer;
    public float lasTime = 12f;
    [NonSerialized] public float ltimer;
    public float spellSpeed;

    public bool spell = false;

    public bool projCast = false;
    public bool atkCast = false;
    public bool lasCast = false;
    public bool tuto = false;
    public bool pause = false;

    public bool stun = false;
    float stunTime = 2f;

    public int phaseMult = 1;

    Animator controller;

    //bool faceR;
    // Start is called before the first frame update
    void Start()
    { 
        CurrentSpells = new List<GameObject>();
        projTime -= phaseMult;
        atkTime -= phaseMult;
        lasTime -= phaseMult;
        ptimer = projTime;
        atimer = atkTime;
        ltimer = lasTime;
        spellSpeed = 5f;
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tuto) return;
        if(pause) return;

        if (stun)
        {
            stunTime -= Time.deltaTime;
            if(stunTime <= 0)
            {
                stun = false;
            }
            return;
        }

        /*if (Player.transform.position.x < this.transform.position.x)
            faceR = true;
        else
            faceR = false;*/

        ptimer -= Time.deltaTime;
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

        if(phaseMult > 0)
        {
            ltimer -= Time.deltaTime;
            if(ltimer <= 0)
            {
                StartCoroutine(CastLas());
                ltimer = lasTime + Random.Range(0, 2);
            }
        }
        
    }

    public void Stunned()
    {
        VFXManager.Instance.PlayEffectOn("Stun", this.gameObject);
        UIManager.Instance.FloatingText(this.transform.position, "stun", true, this.gameObject,Color.yellow,12);
        stun = true;
        stunTime = 2f;
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
    }

    private void OnDestroy()
    {
        foreach(GameObject go in CurrentSpells)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
    }

    public IEnumerator CastProj()
    {
        while (spell) yield return new WaitForEndOfFrame();
        spell = true;
        SoundManager.Instance.Play("spell");
        VFXManager.Instance.PlayEffectOn("Circle", this.gameObject);
        controller.Play("SpellCast", 0);

        yield return new WaitUntil(ProjCast);

        GameObject tmp = Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
        tmp.GetComponent<MageProjectile>().body.velocity = (new Vector2(Player.transform.position.x - this.transform.position.x, Player.transform.position.y - this.transform.position.y).normalized) * spellSpeed;
        CurrentSpells.Add(tmp);

        if(phaseMult == 2)
        {
            yield return new WaitForSeconds(0.15f);
            GameObject bis = Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), bis.GetComponent<Collider2D>());
            bis.GetComponent<MageProjectile>().body.velocity = tmp.GetComponent<MageProjectile>().body.velocity;
            CurrentSpells.Add(bis);
        }

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
        GameObject tmp = Instantiate(AttackPrefab, this.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
        CurrentSpells.Add(tmp);

        atkCast = false;
        spell = false;
        yield return null;
    }

    public IEnumerator CastLas()
    {
        while (spell) yield return new WaitForEndOfFrame();
        spell = true;
        //son
        VFXManager.Instance.PlayEffectAt("Charge",this.transform);
        controller.Play("SpellCast", 0); // à chg?
        yield return new WaitUntil(ProjCast); //idm
        GameObject tmp = Instantiate(LaserPrefab,this.transform.position, Quaternion.identity);
        tmp.transform.right = new Vector2(Player.transform.position.x - tmp.transform.position.x,Player.transform.position.y-tmp.transform.position.y);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), tmp.GetComponentInChildren<Collider2D>());
        CurrentSpells.Add(tmp);

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
        UIManager.Instance.FloatingText(this.transform.position, "", true, this.gameObject);
    }

    public bool ProjCast()
    {
        return projCast;
    }
    public bool AtkCast()
    {
        return atkCast;
    }
    public bool LasCast()
    {
        return lasCast;
    }
}
