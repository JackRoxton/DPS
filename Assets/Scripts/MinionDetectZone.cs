using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDetectZone : MonoBehaviour
{
    public Minion minion;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Player>() != null && !minion.dmgFlag && !minion.stop && !minion.stun)
        {
            StartCoroutine(minion.Attack());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Player>() != null && !minion.dmgFlag && !minion.stop && !minion.stun)
        {
            StartCoroutine(minion.Attack());
        }
    }
}
