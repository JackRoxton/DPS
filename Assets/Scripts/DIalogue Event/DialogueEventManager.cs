using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventManager : Singleton<DialogueEventManager>
{
    public GameObject magePrefab;
    GameObject mage;
    public GameObject minionPrefab;
    GameObject minion;
    public GameObject player;
    public Transform mageSpot;
    public Transform minionSpot;
    public bool eventLock = false;

    public void Event(string sentence)
    {
        UIManager.Instance.DialoguePanel.SetActive(false);
        StartCoroutine(_Event(sentence));
    }

    public IEnumerator _Event(string sentence)
    {
        minion = null;
        UIManager.Instance.DialoguePanel.SetActive(true);
        switch (sentence)
        {
            case "Start":
                UIManager.Instance.dps.locked = true;
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "SpawnMage":
                eventLock = true;
                mage = Instantiate(magePrefab, mageSpot.position, Quaternion.identity);
                mage.GetComponent<Mage>().tuto = true;
                mage.GetComponent<Mage>().timer = 2f;
                mage.GetComponent<Mage>().spellTimer = 2f;
                mage.GetComponent<Mage>().Player = player;
                yield return new WaitForSeconds(1);
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "WaitSlash":
                eventLock = true;
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetS);
                UIManager.Instance.dps.locked = true;
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "WaitDodge":
                eventLock = true;
                mage.GetComponent<Mage>().tuto = false;
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetD);
                mage.GetComponent<Mage>().tuto = true;
                UIManager.Instance.dps.locked = true;
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "WaitParry":
                eventLock = true;
                minion = Instantiate(minionPrefab, minionSpot.position, Quaternion.identity);
                minion.GetComponent<Minion>().player = player;
                minion.GetComponent<Minion>().invincible = true;
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetP);
                UIManager.Instance.dps.locked = true;
                Destroy(minion);
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "End":
                eventLock = true;
                UIManager.Instance.dps.locked = false;
                TeleportEffect.Instance.Effect();
                yield return new WaitForSeconds(1.9f);
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "EndPhase":
                if(GameManager.Instance.phase <= 0)
                    GameManager.Instance.EndGame();
                else
                    UIManager.Instance.EndPhase();

                UIManager.Instance.DialoguePanel.SetActive(false);
                //StopCoroutine(_Event(sentence));
                break;
        }
        /*eventLock = false;
        DialogueManager.Instance.DisplayNextSentence();*/
        //UIManager.Instance.DialoguePanel.SetActive(false);
    }
}
