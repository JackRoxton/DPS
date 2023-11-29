using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClick : MonoBehaviour
{
    void Update()
    {
        if (UIManager.Instance.DialogueIsActive()){
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                UIManager.Instance.NextDialogue();
        }
    }

}
