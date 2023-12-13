using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    float shakeDuration = 0.3f;
    public AnimationCurve shake;
    public AnimationCurve strongShake;
    bool inShake = false;
    Vector3 startPos = Vector3.zero;

    public void ScreenShake()
    {
        if (inShake)
        {
            StopAllCoroutines();
            transform.position = startPos;
        }
        StartCoroutine(_ScreenShake()); 
    }

    public IEnumerator _ScreenShake()
    {
        inShake = true;
        startPos = transform.position;
        float time = 0f;
        while(time < shakeDuration)
        {
            time += Time.deltaTime;
            float strength = shake.Evaluate(time/ shakeDuration);
            transform.position = startPos+Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
        inShake = false;
    }

    public void StrongScreenShake()
    {
        if (inShake)
        {
            StopAllCoroutines();
            transform.position = startPos;
        }
        StartCoroutine(_StrongScreenShake());
    }

    public IEnumerator _StrongScreenShake()
    {
        inShake = true;
        startPos = transform.position;
        float time = 0f;
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            float strength = strongShake.Evaluate(time / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
        inShake = false;
    }

}
