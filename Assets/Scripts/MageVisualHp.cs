using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageVisualHp : MonoBehaviour
{
    public Material mat;
    float blinkTimer = 2f;
    public AnimationCurve fade;
    float str = 1;

    void Start()
    {
        StartCoroutine(Blink());
    }

    public IEnumerator Blink()
    {
         mat = this.gameObject.GetComponent<SpriteRenderer>().sharedMaterial;
        
        float time = blinkTimer;
        while (time > 0)
        {
            time -= Time.deltaTime * str;
            float strength = fade.Evaluate(time / blinkTimer);
            mat.SetFloat("_Thickness", strength);
            yield return null;
        }
        time = 0f;
        while (time < blinkTimer)
        {
            time += Time.deltaTime * str;
            float strength = fade.Evaluate(time / blinkTimer);
            mat.SetFloat("_Thickness", strength);
            yield return null;
        }
        yield return new WaitForSeconds(blinkTimer/str);
        ResetVal();
    }

    public void ResetVal()
    {
        str = 5000 / (GameManager.Instance.mageHP - GameManager.Instance.score);
        StartCoroutine (Blink());
    }
}
