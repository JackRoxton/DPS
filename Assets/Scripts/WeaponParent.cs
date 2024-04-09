using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class WeaponParent : MonoBehaviour
{
    public Vector3 mousePos;
    public bool faceR = true;
    public GameObject player;
    //public GameObject weapon;
    Vector3 memPos;

    [NonSerialized]public bool controller = false;
    [NonSerialized]public bool autoaim = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Controller") == 1)
            controller = true;
        else
            controller = false;

        if (PlayerPrefs.GetInt("AutoAim") == 1)
            autoaim = true;
        else
            autoaim = false;
    }

    private void Update()
    {
        if (player.GetComponent<Player>().currentState == Player.states.Dialogue || player.GetComponent<Player>().pause || player.GetComponent<Player>().endFlag)
            return;

        if (GameManager.Instance.currentState == GameManager.gameStates.InGame 
            || GameManager.Instance.currentState == GameManager.gameStates.Tutorial) 
        {
            if(controller || autoaim)
            {
                if (RoomManager.Instance.mage == null) return;
                List<GameObject> list = RoomManager.Instance.minionList;
                GameObject target = RoomManager.Instance.mage;
                foreach(GameObject go in list)
                {
                    if (go == null) return;
                    if(Vector3.Distance(go.transform.position,this.transform.position) < Vector3.Distance(target.transform.position, this.transform.position))
                    {
                        target = go;
                    }
                }

                Vector3 dir = new Vector3(target.transform.position.x - player.transform.position.x, target.transform.position.y - player.transform.position.y, player.transform.position.z).normalized;

                if (target.transform.position.x < player.transform.position.x && faceR)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
                    faceR = false;
                }
                else if (target.transform.position.x >= player.transform.position.x && !faceR)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
                    faceR = true;
                }
                transform.right = dir;
            }
            else
            {
                mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(player.transform.position).x, Camera.main.WorldToScreenPoint(player.transform.position).y, 0);
                Vector3 dir = new Vector3(mousePos.x - player.transform.position.x,mousePos.y - player.transform.position.y, player.transform.position.z).normalized;
            
                transform.right = dir;
                //Debug.Log(dir);

                if (mousePos.x < player.transform.position.x && faceR)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, -1, 1);
                    faceR = false;
                }
                else if(mousePos.x >= player.transform.position.x && !faceR)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, 1, -1);
                    faceR = true;
                }
            }
        }
    }
}
