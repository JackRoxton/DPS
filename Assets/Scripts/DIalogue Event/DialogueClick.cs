using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClick : MonoBehaviour
{
    bool clickCD = false;
    void Update()
    {
        if (UIManager.Instance.DialogueIsActive()){
            if ((Input.GetMouseButtonDown(0) || Input.GetAxis("Submit") != 0 || Input.GetAxis("Slash") != 0) && !clickCD)
            {
                UIManager.Instance.NextDialogue();
                clickCD = true;
            }
            if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Submit") == 0 && Input.GetAxis("Slash") == 0)
                clickCD = false;
        }
    }

}
