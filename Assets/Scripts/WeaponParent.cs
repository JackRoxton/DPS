using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WeaponParent : MonoBehaviour
{
    public Vector3 mousePos;
    bool faceR = true;
    public GameObject player;

    private void Update()
    {
        mousePos = Input.mousePosition - new Vector3(Camera.main.pixelWidth/2,Camera.main.pixelHeight/2,0);
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = new Vector3(mousePos.x - this.transform.position.x,mousePos.y - this.transform.position.y,this.transform.position.z);
        //Debug.Log(mousePos);

        transform.right = dir;

        if (mousePos.x < player.transform.position.x && faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = false;
        }
        else if(mousePos.x > player.transform.position.x && !faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = true;
        }
    }
}
