using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    public List<GameObject> boxes = new List<GameObject>();
    int i = 5;

    public void HPDown()
    {
        boxes[i-1].SetActive(false);
        i--;
    }

    public void Reset()
    {
        i = 5;
        foreach (GameObject obj in boxes)
        {
            obj.SetActive(true);
        }
    }
}
