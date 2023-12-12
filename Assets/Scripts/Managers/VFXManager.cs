using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : Singleton<VFXManager>
{
    public VisualEffect[] effects;

    public AnimationCurve hitStop;
    public float hitStopLength;

    float lifeTime = 0.75f;

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayEffectAt("Dodge", this.transform);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayEffectAt("Dodge", this.transform, true);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayEffectOn("Dodge", player);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayEffectOn("Dodge", player, true);
        }

    }*/

    public void PlayEffectAt(string name, Transform obj, bool flip = false)
    {
        VisualEffect effect = Array.Find(effects, effect => effect.name == name);
        VisualEffect go;

        if (flip)
        {
            Transform trs = obj;
            Quaternion rot = Quaternion.Euler(trs.rotation.x,trs.rotation.y,trs.rotation.z + 180);
            go = Instantiate(effect, trs.position, rot);
        }
        else
        {
            Transform trs = obj;
            Quaternion rot = Quaternion.Euler(trs.rotation.x, trs.rotation.y, trs.rotation.z);
            go = Instantiate(effect, trs.position, rot);
        }

        StartCoroutine(KillEffect(go));
    }

    public void PlayEffectOn(string name, GameObject obj, bool flip = false)
    {
        VisualEffect effect = Array.Find(effects, effect => effect.name == name);
        VisualEffect go;

        if (flip)
        {
            Transform trs = obj.transform;
            Quaternion rot = Quaternion.Euler(trs.rotation.x, trs.rotation.y, trs.rotation.z + 180);
            go = Instantiate(effect, trs.position, rot);
            go.transform.parent = obj.transform;
        }
        else
        {
            Transform trs = obj.transform;
            Quaternion rot = Quaternion.Euler(trs.rotation.x, trs.rotation.y, trs.rotation.z);
            go = Instantiate(effect, trs.position, rot);
            go.transform.parent = obj.transform;
        }

        StartCoroutine(KillEffect(go));
    }

    IEnumerator KillEffect(VisualEffect effect)
    {
        yield return new WaitForSeconds(lifeTime);
        if(effect != null) 
            Destroy(effect.gameObject);
    }

    public void HitStop()
    {
        StartCoroutine(_HitStop());
    }

    IEnumerator _HitStop()
    {
        float timer = 0;
        while (timer < hitStopLength)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = hitStop.Evaluate(timer / hitStopLength);
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return null;
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
    }
}
