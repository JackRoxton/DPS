using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject MainMenuPanel, SkillTreePanel;
    public GameObject InGamePanel, PausePanel;
    public GameObject SettingsPanel; //tuto, end

    public TMP_Text timerText, scoreText, comboText;


    //public List<GameObject> existingObjects = new List<GameObject>();

    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame
    }
    public menuStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = menuStates.MainMenu;
        SkillTreePanel.SetActive(false);
        InGamePanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = GameManager.Instance.score.ToString();
        scoreText.text = GameManager.Instance.combo.ToString();
        comboText.text = GameManager.Instance.Timer.ToString();

        if(Input.GetKeyDown(KeyCode.Keypad1)) { Play(); }
        if(Input.GetKeyDown(KeyCode.Keypad2)) { MainMenu(); }
    }

    public void Play()
    {
        /*foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(InGamePanel));*/
        currentState = menuStates.InGame;

        GameManager.Instance.Play();
    }

    public void MainMenu()
    {
        /*foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(MainMenuPanel));*/
        currentState = menuStates.MainMenu;
    }

    public void SkillTree()
    {
        /*foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(SkillTreePanel));*/
        currentState = menuStates.SkillTree;
    }

    public void PauseMenu()
    {
        //existingObjects.Add(Instantiate(PausePanel));
        currentState = menuStates.Pause;
    }

    public void Settings()
    {
        //existingObjects.Add(Instantiate(PausePanel));
        currentState = menuStates.Settings;
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }

}
