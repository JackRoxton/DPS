using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeDuration = 0.1f;
    public AnimationCurve fade;
    public Image spr;

    public void FadeInOut(int i)
    {
        StartCoroutine(_Fade(i));
    }

    public void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }

    public IEnumerator _Fade(int i = -1)
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
        if(i != -1)
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

    public IEnumerator _FadeIn()
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
    }

    public IEnumerator _FadeOut()
    {
        float time = 0f;
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
