using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public bool hitFlag = true;

    private void Start()
    {
        StartCoroutine(Expand());
    }

    public IEnumerator Expand()
    {
        GameManager.Instance.ScreenShake();
        yield return new WaitForSeconds(0.5f);
        transform.localScale *= 1.5f;
        GameManager.Instance.ScreenShake();
        yield return new WaitForSeconds(0.5f);
        transform.localScale *= 1.5f;
        GameManager.Instance.ScreenShake();
        yield return new WaitForSeconds(0.5f);
        Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
