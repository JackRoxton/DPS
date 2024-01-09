using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Animator controller;
    public AnimationCurve curve;
    public float lifeTime = 2f;
    public float n;

    private void Start()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        float time = 0;
        //Color32 a = GetComponent<TextMesh>().color;
        while (time < lifeTime)
        {
            while (GameManager.Instance.currentState == GameManager.gameStates.Pause) yield return null;
            time += Time.deltaTime;
            float strength = curve.Evaluate(time / lifeTime);
            this.transform.localScale = new Vector3(strength, strength, strength);
            /*Debug.Log(strength);
            a.a = ((byte)strength);
            GetComponent<TextMesh>().color = a;*/
            yield return null;
        }
        Die();
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        this.transform.localScale = Vector3.one;
        /*Color32 a = GetComponent<TextMesh>().color;
        a.a = ((byte)0);
        GetComponent<TextMesh>().color = a;*/
        controller.Play("FloatingText");
        StartCoroutine(Play());
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
