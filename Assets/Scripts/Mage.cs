using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mage : MonoBehaviour
{
    public GameObject SpellPrefab;
    GameObject CurrentSpell;
    [NonSerialized]
    public GameObject Player;
    public float spellTimer = 5f;
    float timer;
    public float spellSpeed;

    public bool spellCast = false;

    Animator controller;

    bool faceR;
    // Start is called before the first frame update
    void Start()
    {
        timer = spellTimer;
        spellSpeed = 5f;
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            spellCast = false;
            StartCoroutine(CastSpell());
            timer = spellTimer + Random.Range(-1, 2);
        }
    }

    public IEnumerator CastSpell()
    {
        SoundManager.Instance.Play("spell");
        controller.Play("SpellCast",0);
        yield return new WaitUntil(SpellCast);
        CurrentSpell = Instantiate(SpellPrefab, this.transform.position,Quaternion.identity);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(),CurrentSpell.GetComponent<Collider2D>());
        CurrentSpell.GetComponent<MageProjectile>().body.velocity = (new Vector2(Player.transform.position.x - this.transform.position.x, Player.transform.position.y -this.transform.position.y).normalized)*spellSpeed;
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

    public bool SpellCast()
    {
        return spellCast;
    }
}
