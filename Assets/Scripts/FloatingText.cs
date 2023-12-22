using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Animator controller;
    public AnimationCurve curve;
    float timer = 2f;
    public float n;

    private void Start()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        float time = 0;
        Color a = GetComponent<TextMesh>().color;
        while (time < timer)
        {
            while (GameManager.Instance.currentState == GameManager.gameStates.Pause) yield return null;
            time += Time.deltaTime;
            float strength = curve.Evaluate(time / timer);
            a.a = strength;
            GetComponent<TextMesh>().color = a;
            yield return null;
        }
        Die();
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        /*Color a = GetComponent<TextMesh>().color;
        a.a = 1;
        GetComponent<TextMesh>().color = a;*/
        controller.Play("FloatingText");
        StartCoroutine(Play());
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
