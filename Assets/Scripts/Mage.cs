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

    bool faceR;
    // Start is called before the first frame update
    void Start()
    {
        timer = spellTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            StartCoroutine(CastSpell());
            timer = spellTimer + Random.Range(-1, 2);
        }
    }

    public IEnumerator CastSpell()
    {
        CurrentSpell = Instantiate(SpellPrefab, this.transform);
        CurrentSpell.GetComponent<MageProjectile>().body.velocity = new Vector2(Player.transform.position.x - this.transform.position.x, Player.transform.position.y -this.transform.position.y);
        yield return null;
    }

    public void Teleport(Transform spot)
    {
        //effet
        this.transform.position = spot.position;
    }

    public void TakeDamage()
    {
        GameManager.Instance.AddScore(10);
    }
}
