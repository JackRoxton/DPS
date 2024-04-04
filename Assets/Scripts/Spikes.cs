using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (collision.gameObject.GetComponent<Player>().currentState != Player.states.Dodge)
            {
                collision.gameObject.GetComponent<Player>().TakeDamage();
            }
        }
    }
}