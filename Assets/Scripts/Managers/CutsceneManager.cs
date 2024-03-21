using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public GameObject Tower;
    private void Start()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        //UIManager.Instance.SkillTreePanel.SetActive(false);
        UIManager.Instance.InGamePanel.SetActive(false);
        UIManager.Instance.DialoguePanel.SetActive(false);
        yield return new WaitForSeconds(1);
        UIManager.Instance.Credits(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //se souvenir de ça et changer
        UIManager.Instance.Credits(true,false);
        TeleportEffect.Instance.Effect();
        yield return new WaitForSeconds(2);
        VFXManager.Instance.PlayEffectAt("Teleport_End", Tower.transform);
        Tower.SetActive(false);
        yield return new WaitForSeconds(2);
        UIManager.Instance.FadeScene(0);
        UIManager.Instance.MainMenuPanel.SetActive(true);
        GameManager.Instance.ToMenu();
    }

    /*bool GetMouseButton()
    {
        return Input.GetMouseButton(0);
    }*/
}
