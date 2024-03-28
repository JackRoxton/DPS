using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool locked = true;
    public GameObject Circle;
    public GameObject Lock;
    public GameObject D, P, S;
    //bool Dstate, Pstate, Sstate;

    public enum Powerup
    {
        Attack,
        Speed,
        AttackSpeed
    }
    public Powerup current;

    private void Start()
    {
        if (UIManager.Instance.dps.GetD())
        {
            SetLightState("D");
        }
        if (UIManager.Instance.dps.GetP())
        {
            SetLightState("P");
        }
        if (UIManager.Instance.dps.GetS())
        {
            SetLightState("S");
        }
    }

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
        if (!locked) return;
        locked = false;
        Lock.SetActive(false);
        VFXManager.Instance.PlayEffectAt("Success",this.transform);
        //SoundManager.Instance.Play("");
    }

    void Take(Player player)
    {
        switch (current)
        {
            case Powerup.Attack:
                GameManager.Instance.PickupStrength();
                UIManager.Instance.FloatingText(player.transform.position, "Strength", false, null, Color.cyan, 8);
            break;
            case Powerup.Speed:
                RoomManager.Instance.PickupSpeed();
                UIManager.Instance.FloatingText(player.transform.position, "Speed", false, null, Color.cyan, 8);
                break;
            case Powerup.AttackSpeed:
                RoomManager.Instance.PickupAttackSpeed();
                UIManager.Instance.FloatingText(player.transform.position, "AttackSpeed", false, null, Color.cyan, 8);
                break;
        }

        player.CirclePowerup(Circle.GetComponent<SpriteRenderer>().color);
        SoundManager.Instance.Play("powerup");
        VFXManager.Instance.PlayEffectAt("Strength", this.transform);
        Destroy(this.gameObject);
    }

    public void SetLightState(string lamp)
    {
        switch(lamp)
        {
            case "D":
                D.GetComponent<SpriteRenderer>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                //Dstate = true;
                break;
            case "P":
                P.GetComponent<SpriteRenderer>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                //Pstate = true;
                break;
            case "S":
                S.GetComponent<SpriteRenderer>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                //Sstate = true;
                break;
        }
    }
}
