using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClick : MonoBehaviour
{
    bool dialogueLock = false;
    void Update()
    {
        if (dialogueLock) return;
        if (UIManager.Instance.DialogueIsActive()){
            if (Input.anyKeyDown)
            {
                UIManager.Instance.NextDialogue();
                LockTimer();
            }
        }
    }

    public void LockTimer()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        dialogueLock = true;
        yield return new WaitForSeconds(0.2f);
        dialogueLock = false;
    }

}
