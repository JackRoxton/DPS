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
            D.GetComponent<Image>().color = Color.yellow;
        else
            D.GetComponent<Image>().color = Color.white;
    }

    public void PLight(bool state)
    {
        if (state == Pstate) return;

        if (state)
            P.GetComponent<Image>().color = Color.yellow;
        else
            P.GetComponent<Image>().color = Color.white;
    }

    public void SLight(bool state)
    {
        if (state == Sstate) return;

        if (state)
            S.GetComponent<Image>().color = Color.yellow;
        else
            S.GetComponent<Image>().color = Color.white;
    }

    public void ResetLights()
    {
        GameManager.Instance.DPSCycle();
        DLight(false);
        PLight(false);
        SLight(false);
    }
}
