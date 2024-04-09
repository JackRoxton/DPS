using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().currentState != Player.states.Dodge)
            {
                collision.gameObject.GetComponent<Player>().TakeDamage();
            }
        }
    }*/
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject.tag == "Player" && player && !collision.isTrigger)
        {
            if (player.currentState != Player.states.Dodge)
            {
                player.TakeDamage();
            }
        }
    }
}
