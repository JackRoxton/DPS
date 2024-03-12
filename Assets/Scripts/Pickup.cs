using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool locked = true;
    public GameObject Circle;
    public GameObject Lock;

    public enum Powerup
    {
        Attack,
        Speed,
        AttackSpeed
    }
    public Powerup current;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (locked) return;

        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            Take(player);
        }
    }

    public void Unlock()
    {
        locked = false;
        Lock.SetActive(false);
    }

    void Take(Player player)
    {
        switch (current)
        {
            case Powerup.Attack:
                GameManager.Instance.PickupStrength();
            break;
            case Powerup.Speed:
                RoomManager.Instance.PickupSpeed();
            break;
            case Powerup.AttackSpeed:
                RoomManager.Instance.PickupAttackSpeed();
                break;
        }

        player.CirclePowerup(Circle.GetComponent<SpriteRenderer>().color);
        SoundManager.Instance.Play("powerup");
        VFXManager.Instance.PlayEffectAt("Strength", this.transform);
        Destroy(this.gameObject);
    }
}
