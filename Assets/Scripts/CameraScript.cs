using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float duration = 0.2f;
    public AnimationCurve shake;

    public void ScreenShake()
    {
        StartCoroutine(_ScreenShake()); 
    }

    public IEnumerator _ScreenShake()
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            float strength = shake.Evaluate(time/duration);
            transform.position = startPos+Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
    }
}
