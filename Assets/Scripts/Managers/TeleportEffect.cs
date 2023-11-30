using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : Singleton<TeleportEffect>
{
    public GameObject Effects;
    public AnimationCurve alpha;
    float duration = 2f;

    public GameObject Ripple;
    SpriteRenderer RSr;

    public GameObject Distortion;
    SpriteRenderer DSr;

    private void Start()
    {
        RSr = Ripple.GetComponent<SpriteRenderer>();
        DSr = Distortion.GetComponent<SpriteRenderer>();
    }

    public void Effect()
    {
        StartCoroutine(_Effect());
    }

    public IEnumerator _Effect()
    {
        float time = 0f;
        float _alpha = 0f;
        Color Rcolor = RSr.color;
        Color Dcolor = DSr.color;
        while (time < duration)
        {
            time += Time.deltaTime;
            _alpha = alpha.Evaluate(time/duration);
            Rcolor.a = _alpha/12;
            Dcolor.a = _alpha*15;
            RSr.color = Rcolor;
            DSr.color = Dcolor;
            
            yield return null;
        }
        Rcolor.a = 0;
        Dcolor.a = 0;
        RSr.color = Rcolor;
        DSr.color = Dcolor;
    }
}
