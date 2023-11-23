using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    float lifeTimer = 1.9f;
    bool playerInHb = false;
    bool minionInHb = false;
    GameObject player;
    GameObject minion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer < 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(playerInHb)
            player.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0) * 20f;
        if (minionInHb)
            minion.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0) * 20f;

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.GetComponent<Player>() != null) 
        {
            playerInHb = true;
            player = collision.gameObject;
        }

        if (collision.GetComponent<Minion>() != null)
        {
            minionInHb = true;
            minion = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.GetComponent<Player>() != null) playerInHb = false;

        if (collision.GetComponent<Minion>() != null) minionInHb = false;
    }
}
