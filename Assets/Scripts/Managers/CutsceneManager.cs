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
        yield return new WaitForSeconds(1);
        UIManager.Instance.Credits(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //se souvenir de �a et changer
        UIManager.Instance.Credits(true,false);
        VFXManager.Instance.PlayEffectAt("Teleport_End", Tower.transform);
        yield return new WaitForSeconds(1);
        Tower.SetActive(false);
        yield return new WaitForSeconds(1);
        UIManager.Instance.FadeScene(0);
        UIManager.Instance.MainMenuPanel.SetActive(true);
        GameManager.Instance.ToMenu();
    }

    /*bool GetMouseButton()
    {
        return Input.GetMouseButton(0);
    }*/
}
