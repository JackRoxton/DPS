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

    public void Event(string sentence)
    {
        UIManager.Instance.DialoguePanel.SetActive(false);
        StartCoroutine(_Event(sentence));
    }

    public IEnumerator _Event(string sentence)
    {
        switch(sentence)
        {
            case "Start":
                UIManager.Instance.dps.locked = true;
                break;
            case "SpawnMage":
                mage = Instantiate(magePrefab, mageSpot.position, Quaternion.identity);
                mage.GetComponent<Mage>().tuto = true;
                mage.GetComponent<Mage>().timer = 1f;
                mage.GetComponent<Mage>().spellTimer = 1f;
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
        }
        UIManager.Instance.DialoguePanel.SetActive(true);
        DialogueManager.Instance.DisplayNextSentence();
    }
}
