using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClick : MonoBehaviour
{
    bool dialogueLock = false;

    private void OnEnable()
    {
        LockTimer();
    }

    void Update()
    {
        if (!this.gameObject.activeInHierarchy) return;
        if (dialogueLock) return;
        if (UIManager.Instance.DialogueIsActive()){
            if (Input.anyKeyDown)
            {
                LockTimer();
                SoundManager.Instance.Play("next");
                UIManager.Instance.NextDialogue();
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
        yield return new WaitForSeconds(0.3f);
        dialogueLock = false;
    }

}
