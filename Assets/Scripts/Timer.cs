using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.globalTimer < 30)
        {
            if (GameManager.Instance.globalTimer < 10)
            {
                this.GetComponent<Image>().color = Color.red;
                this.GetComponent<Animator>().speed = 0.5f;
                return;
            }
            this.GetComponent<Image>().color = new Color(1,0.5f,0);
            this.GetComponent<Animator>().speed = 0.25f;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
            this.GetComponent<Animator>().speed = 0.1f;
        }
    }
}
