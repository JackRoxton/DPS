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
    public DPSCycle dps;

    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame
    }
    public menuStates currentState;

    new void Awake()
    {
        currentState = menuStates.MainMenu;
        MainMenuPanel.SetActive(true);
        SkillTreePanel.SetActive(false);
        InGamePanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == menuStates.InGame)
        {
            timerText.text = ((int)GameManager.Instance.Timer).ToString();
            scoreText.text = GameManager.Instance.score.ToString();
            comboText.text = GameManager.Instance.combo.ToString();
        }

    }

    public void Play()
    {
        currentState = menuStates.InGame;

        GameManager.Instance.Play();

        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
    }

    public void Resume()
    {
        currentState = menuStates.InGame;

        GameManager.Instance.Resume();

        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void MainMenu()
    {
        currentState = menuStates.MainMenu;

        MainMenuPanel.SetActive(true);
        InGamePanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void Pause()
    {
        currentState = menuStates.Pause;
        PausePanel.SetActive(true);
    }

    public void SkillTree()
    {
        currentState = menuStates.SkillTree;
    }

    public void Settings()
    {
        currentState = menuStates.Settings;
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }

}
