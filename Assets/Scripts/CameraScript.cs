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

    public void ScreenShake()
    {
        StartCoroutine(_ScreenShake()); 
    }

    public IEnumerator _ScreenShake()
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        while(time < shakeDuration)
        {
            time += Time.deltaTime;
            float strength = shake.Evaluate(time/ shakeDuration);
            transform.position = startPos+Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
    }

    public void StrongScreenShake()
    {
        StartCoroutine(_StrongScreenShake());
    }

    public IEnumerator _StrongScreenShake()
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            float strength = strongShake.Evaluate(time / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
    }

}
