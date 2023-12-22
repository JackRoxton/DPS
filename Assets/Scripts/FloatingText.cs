using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Animator controller;
    float timer = 2f;
    public float n;

    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.gameStates.Pause) return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Die();
        }
    }

    public void ResetTimer()
    {
        timer = 2f;
        controller.Play("FloatingText");
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
