using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public List<GameObject> boxes = new List<GameObject>();
    int i = 5;
    public Color c;

    public void Start()
    {
        //c = boxes[i-1].GetComponent<Image>().color;
        //c = Color.red;
        //Debug.Log(c);
    }

    public void HPDown()
    {
        boxes[i-1].GetComponent<Image>().color = Color.white;
        boxes[i-1].GetComponent<Animator>().Play("lifebump",0);
        i--;
    }

    public void HPBump()
    {
        boxes[i - 1].GetComponent<Animator>().Play("lifebump", 0);
    }

    public void Resetvar()
    {
        i = 5;
        //Debug.Log(c);
        foreach (GameObject obj in boxes)
        {
            obj.GetComponent<Image>().color = c;
        }
    }
}
