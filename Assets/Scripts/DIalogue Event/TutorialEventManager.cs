using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventManager : Singleton<TutorialEventManager>
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
        UIManager.Instance.DialoguePanel.SetActive(true);
        eventLock = true;
        switch (sentence)
        {
            case "Start":
                UIManager.Instance.dps.locked = true;
                break;
            case "SpawnMage":
                mage = Instantiate(magePrefab, mageSpot.position, Quaternion.identity);
                mage.GetComponent<Mage>().tuto = true;
                mage.GetComponent<Mage>().timer = 2f;
                mage.GetComponent<Mage>().spellTimer = 2f;
                mage.GetComponent<Mage>().Player = player;
                yield return new WaitForSeconds(1);
                break;
            case "WaitSlash":
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetS);
                UIManager.Instance.dps.locked = true;
                break;
            case "WaitDodge":
                mage.GetComponent<Mage>().tuto = false;
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetD);
                mage.GetComponent<Mage>().tuto = true;
                UIManager.Instance.dps.locked = true;
                break;
            case "WaitParry":
                minion = Instantiate(minionPrefab, minionSpot.position, Quaternion.identity);
                minion.GetComponent<Minion>().player = player;
                minion.GetComponent<Minion>().invincible = true;
                UIManager.Instance.dps.locked = false;
                yield return new WaitUntil(UIManager.Instance.dps.GetP);
                UIManager.Instance.dps.locked = true;
                Destroy(minion);
                break;
            case "End":
                UIManager.Instance.dps.locked = false;
                TeleportEffect.Instance.Effect();
                yield return new WaitForSeconds(1.9f);
                break;
        }
        eventLock = false;
        DialogueManager.Instance.DisplayNextSentence();
    }
}
