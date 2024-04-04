using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueEventManager : Singleton<DialogueEventManager>
{
    /*public GameObject magePrefab;
    GameObject mage;
    public GameObject minionPrefab;
    GameObject minion;
    public GameObject player;
    public Transform mageSpot;
    public Transform minionSpot;*/
    public bool eventLock = false;
    public void Event(string sentence)
    {
        UIManager.Instance.DialoguePanel.SetActive(false);
        StartCoroutine(_Event(sentence));
    }

    public IEnumerator _Event(string sentence)
    {
        //minion = null;
        UIManager.Instance.DialoguePanel.SetActive(true);
        switch (sentence)
        {
            /*case "Start":
                UIManager.Instance.dps.locked = true;
                eventLock = false;
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "SpawnMage":
                eventLock = true;
                mage = Instantiate(magePrefab, mageSpot.position, Quaternion.identity);
                mage.GetComponent<Mage>().tuto = true;
                mage.GetComponent<Mage>().ptimer = 2f;
                mage.GetComponent<Mage>().projTime = 2f;
                mage.GetComponent<Mage>().Player = player;
                UIManager.Instance.DialoguePanel.SetActive(false);
                yield return new WaitForSeconds(1);
                UIManager.Instance.DialoguePanel.SetActive(true);
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
            case "Attack":
                UIManager.Instance.DialoguePanel.SetActive(false);
                eventLock = true;
                mage.GetComponent<Animator>().Play("AtkCast",0);
                SoundManager.Instance.Play("attack");
                GameObject go = Instantiate(mage.GetComponent<Mage>().AttackPrefab,mage.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(2f);
                eventLock = false;
                UIManager.Instance.DialoguePanel.SetActive(true);
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
                break;*/
            case "EndPhase":
                if(GameManager.Instance.phase <= 0)
                    GameManager.Instance.EndGame();
                else
                    UIManager.Instance.EndPhase();

                UIManager.Instance.DialoguePanel.SetActive(false);
                //StopCoroutine(_Event(sentence));
                break;
            case "TutoStart":
                UIManager.Instance.dps.D.SetActive(false);
                UIManager.Instance.dps.P.SetActive(false);
                UIManager.Instance.dps.S.SetActive(false);
                //UIManager.Instance.timer.gameObject.SetActive(false);
                DialogueManager.Instance.DisplayNextSentence();
                RoomManager.Instance.tuto = true;
                RoomManager.Instance.Pause(true);
                //RoomManager.Instance.mage.GetComponent<Mage>().tuto = true;
                break;
            case "TutoSlash":
                RoomManager.Instance.Pause(false);
                TeleportEffect.Instance.Effect();
                UIManager.Instance.DialoguePanel.SetActive(false);
                yield return new WaitForSeconds(1.9f);
                RoomManager.Instance.FirstRoom();
                GameManager.Instance.firstRoom = false;
                UIManager.Instance.dps.S.SetActive(true);
                UIManager.Instance.SetControlText("Slash");
                GameManager.Instance.globalTimer = 30f;
                yield return new WaitForSeconds(30);
                RoomManager.Instance.Pause(true);
                UIManager.Instance.DialoguePanel.SetActive(true);
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "TutoDodge":
                RoomManager.Instance.Pause(false);
                RoomManager.Instance.mage.GetComponent<Mage>().tuto = false;
                UIManager.Instance.dps.D.SetActive(true);
                UIManager.Instance.DialoguePanel.SetActive(false);
                UIManager.Instance.SetControlText("Dodge");
                GameManager.Instance.globalTimer = 30f;
                yield return new WaitForSeconds(30);
                RoomManager.Instance.Pause(true);
                UIManager.Instance.DialoguePanel.SetActive(true);
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "TutoParry":
                RoomManager.Instance.Pause(false);
                RoomManager.Instance.tuto = false;
                UIManager.Instance.dps.P.SetActive(true);
                UIManager.Instance.DialoguePanel.SetActive(false);
                UIManager.Instance.SetControlText("Parry");
                GameManager.Instance.globalTimer = 30f;
                yield return new WaitForSeconds(30);
                RoomManager.Instance.Pause(true);
                UIManager.Instance.DialoguePanel.SetActive(true);
                DialogueManager.Instance.DisplayNextSentence();
                break;
            case "TutoEnd":
                eventLock = true;
                UIManager.Instance.FloatingText(new Vector2(0, 0), "Tutorial Completed", false, null, Color.blue,20);
                yield return new WaitForSeconds(1.5f);
                UIManager.Instance.FloatingText(new Vector2(0, 0), "Now the Real Deal", false, null, Color.blue, 20);
                TeleportEffect.Instance.Effect();
                UIManager.Instance.DialoguePanel.SetActive(false);
                yield return new WaitForSeconds(1.9f);
                eventLock = false;
                //UIManager.Instance.timer.gameObject.SetActive(true);
                DialogueManager.Instance.DisplayNextSentence();
                break;
        }
        /*eventLock = false;
        DialogueManager.Instance.DisplayNextSentence();
        UIManager.Instance.DialoguePanel.SetActive(false);*/
    }
}
