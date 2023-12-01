using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageVisualHp : MonoBehaviour
{
    public Material mat;
    float blinkTimer = 1f;
    public AnimationCurve fade;
    float str = 1;

    void Start()
    {
        StartCoroutine(Blink());
    }

    public IEnumerator Blink()//marche po, au lieu de l'alpha plutôt la force du contour ?
    {
        float time = blinkTimer;
        while (time > 0)
        {
            time -= Time.deltaTime * str;
            float strength = fade.Evaluate(time / blinkTimer);
            Color a = mat.color;
            a.a = strength;
            mat.color = a;
            yield return null;
        }
        time = 0f;
        while (time < blinkTimer)
        {
            time += Time.deltaTime * str;
            float strength = fade.Evaluate(time / blinkTimer);
            Color a = mat.color;
            a.a = strength;
            mat.color = a;
            yield return null;
        }
        ResetVal();
    }

    public void ResetVal()
    { 
        //str =
        StartCoroutine (Blink());
    }
}
