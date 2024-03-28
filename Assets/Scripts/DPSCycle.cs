using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSCycle : MonoBehaviour
{
    public GameObject D, P, S;
    bool Dstate = false;
    bool Pstate = false;
    bool Sstate = false;
    //bool doneOnce = false;
    public bool locked = false;


    public void Light(string name, bool state)
    {
        if (locked)
        {
            return;
        }
        switch (name)
        {
            case "D":
                if (state == Dstate) return;
                if (state)
                {
                    D.GetComponent<Image>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                    Dstate = true;
                    D.GetComponent<Animator>().Play("DBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    D.GetComponent<Image>().color = new Color32(0xd1,0xd1,0xd1,255);
                    Dstate = false;
                }
                break;
            case "P":
                if (state == Pstate) return;
                if (state)
                {
                    P.GetComponent<Image>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                    Pstate = true;
                    P.GetComponent<Animator>().Play("PBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    P.GetComponent<Image>().color = new Color32(0xd1, 0xd1, 0xd1, 255);
                    Pstate = false;
                }
                break;
            case "S":
                if (state == Sstate) return;
                if (state)
                {
                    S.GetComponent<Image>().color = new Color32(0xf8, 0xe0, 0x28, 255);
                    Sstate = true;
                    S.GetComponent<Animator>().Play("SBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    S.GetComponent<Image>().color = new Color32(0xd1, 0xd1, 0xd1, 255);
                    Sstate = false;
                }
                break;
        }
        if(RoomManager.Instance != null)
            RoomManager.Instance.SetPickupLightState();
        CheckLights();
    }

    public bool GetState(string name)
    {
        switch (name)
        {
            case "D":
                return Dstate;
            case "P":
                return Pstate;
            case "S":
                return Sstate;
        }
        return false;
    }

    public bool GetD()
    {
        return Dstate;
    }
    public bool GetP()
    {
        return Pstate;
    }
    public bool GetS()
    {
        return Sstate;
    }

    public void CheckLights()
    {
        if(Dstate && Pstate && Sstate)
        {
            S.GetComponent<Animator>().Play("SBump");
            D.GetComponent<Animator>().Play("DBump");
            P.GetComponent<Animator>().Play("PBump");
            ResetLights();
            GameManager.Instance.DPSCycle();
            SoundManager.Instance.Play("dps");
            if(RoomManager.Instance != null)
                VFXManager.Instance.PlayEffectAt("Shockwave", RoomManager.Instance.player.transform);
            else
                VFXManager.Instance.PlayEffectAt("Shockwave", VFXManager.Instance.transform);
            VFXManager.Instance.HitStop();
            UIManager.Instance.dpsTimes.GetComponent<Animator>().Play("MultBump");
            RoomManager.Instance.player.GetComponent<Player>().Burst();
            GameManager.Instance.Burst();
            //doneOnce = true;
        }
    }

    public void ResetLights()
    {
        Light("D",false);
        Light("P",false);
        Light("S",false);
    }
}
