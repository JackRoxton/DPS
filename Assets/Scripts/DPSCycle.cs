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

    public void DLight(bool state)
    {
        if(state == Dstate) return;

        if (state)
        {
            D.GetComponent<Image>().color = Color.yellow;
            Dstate = true;
        }
        else
        {
            D.GetComponent<Image>().color = Color.white;
            Dstate = false;
        }

        CheckLights();
    }

    public void PLight(bool state)
    {
        if (state == Pstate) return;

        if (state)
        {
            P.GetComponent<Image>().color = Color.yellow;
            Pstate = true;
        }
        else
        {
            P.GetComponent<Image>().color = Color.white;
            Pstate = false;
        }

        CheckLights();
    }

    public void SLight(bool state)
    {
        if (state == Sstate) return;

        if (state)
        {
            S.GetComponent<Image>().color = Color.yellow;
            Sstate = true;
        }
        else
        {
            S.GetComponent<Image>().color = Color.white;
            Sstate = false;
        }

        CheckLights();
    }

    public void CheckLights()
    {
        if(Dstate && Pstate && Sstate)
        {
            ResetLights();
            GameManager.Instance.DPSCycle();
            SoundManager.Instance.Play("dps");
        }
    }

    public void ResetLights()
    {
        DLight(false);
        PLight(false);
        SLight(false);
    }
}
