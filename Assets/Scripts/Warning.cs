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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            return;
        }
        if (GameManager.Instance.endFlag)
        {
            Die();
        }
        lifeTimer -= Time.deltaTime;
        if(lifeTimer < 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (playerInHb)
        {
            player.GetComponent<Rigidbody2D>().velocity = (player.transform.position - this.transform.position).normalized * 30f;
        }

        if (minionInHb)
            minion.GetComponent<Rigidbody2D>().velocity = (minion.transform.position - this.transform.position).normalized * 30f;

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

        if (collision.isTrigger) return;
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

        if (collision.isTrigger) return;
        if (collision.GetComponent<Minion>() != null) minionInHb = false;
    }
}
