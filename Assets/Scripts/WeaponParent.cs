using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 mousePos;
    bool faceR = true;
    public GameObject player;

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = new Vector2(mousePos.x - this.transform.position.x,mousePos.y - this.transform.position.y);

        transform.right = dir;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < player.transform.position.x && faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = false;
        }
        else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > player.transform.position.x && !faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = true;
        }
    }
}
