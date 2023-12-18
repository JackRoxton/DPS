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
    bool doneOnce = false;
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
                    D.GetComponent<Image>().color = Color.yellow;
                    Dstate = true;
                    D.GetComponent<Animator>().Play("DBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    D.GetComponent<Image>().color = Color.white;
                    Dstate = false;
                }
                break;
            case "P":
                if (state == Pstate) return;
                if (state)
                {
                    P.GetComponent<Image>().color = Color.yellow;
                    Pstate = true;
                    P.GetComponent<Animator>().Play("PBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    P.GetComponent<Image>().color = Color.white;
                    Pstate = false;
                }
                break;
            case "S":
                if (state == Sstate) return;
                if (state)
                {
                    S.GetComponent<Image>().color = Color.yellow;
                    Sstate = true;
                    S.GetComponent<Animator>().Play("SBump");
                    SoundManager.Instance.Play("light");
                }
                else
                {
                    S.GetComponent<Image>().color = Color.white;
                    Sstate = false;
                }
                break;
        }
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
        return doneOnce;//pour tuto
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
            VFXManager.Instance.PlayEffectAt("Shockwave", RoomManager.Instance.player.transform);
            VFXManager.Instance.HitStop();
            doneOnce = true;
        }
    }

    public void ResetLights()
    {
        Light("D",false);
        Light("P",false);
        Light("S",false);
    }
}
