using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Powerup
    {
        Attack,
        Speed,
        AttackSpeed
    }
    public Powerup current;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            Take();
        }
    }

    void Take()
    {
        switch (current)
        {
            case Powerup.Attack:
                GameManager.Instance.playerAttack += 5;
            break;
            case Powerup.Speed:
                RoomManager.Instance.PlayerSpeed(0.01f);
            break;
            case Powerup.AttackSpeed:
                RoomManager.Instance.PlayerAttackSpeed(0.05f);
                break;
        }

        SoundManager.Instance.Play("powerup");
        VFXManager.Instance.PlayEffectAt("strength", this.transform);
        StartCoroutine(DieCD());
        //son
    }

    IEnumerator DieCD()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }
}
