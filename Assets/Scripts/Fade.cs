using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeDuration = 0.4f;
    public AnimationCurve fade;
    public Image spr;

    public void FadeOut(int i)
    {
        StartCoroutine(_Fade(i));
    }

    public IEnumerator _Fade(int i)
    {
        float time = fadeDuration;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float strength = fade.Evaluate(time / fadeDuration);
            Color a = spr.color;
            a.a = strength;
            spr.color = a;
            yield return null;
        }
        GameManager.Instance.ChangeScene(i);
        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float strength = fade.Evaluate(time / fadeDuration);
            Color a = spr.color;
            a.a = strength;
            spr.color = a;
            yield return null;
        }
    }
}
