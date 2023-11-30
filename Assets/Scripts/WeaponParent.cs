using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WeaponParent : MonoBehaviour
{
    public Vector3 mousePos;
    public bool faceR = true;
    public GameObject player;

    private void Update()
    {
        if(GameManager.Instance.currentState == GameManager.gameStates.InGame 
            || GameManager.Instance.currentState == GameManager.gameStates.Tutorial) 
        {
            mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(player.transform.position).x, Camera.main.WorldToScreenPoint(player.transform.position).y, 0);
            Vector3 dir = new Vector3(mousePos.x - player.transform.position.x,mousePos.y - player.transform.position.y, player.transform.position.z).normalized;

            transform.right = dir;

            if (mousePos.x < player.transform.position.x && faceR)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
                faceR = false;
            }
            else if(mousePos.x >= player.transform.position.x && !faceR)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
                faceR = true;
            }
        }
    }
}
