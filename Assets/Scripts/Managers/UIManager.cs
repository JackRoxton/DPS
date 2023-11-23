using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject MainMenuPanel, SkillTreePanel;
    public GameObject InGamePanel, PausePanel, EndPanel, CreditsPanel;
    public GameObject SettingsPanel; //tuto, dialogue

    public TMP_Text timerText, scoreText, comboText;
    public TMP_Text endComboText, endScoreText, endDpsText;
    public TMP_Text moneyText;
    public DPSCycle dps;

    public Slider masterSlider, musicsSlider, effectsSlider;

    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame
    }
    public menuStates currentState = menuStates.MainMenu;

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
            timerText.text = ((int)GameManager.Instance.globalTimer).ToString();
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

    public void PlayShort()
    {
        currentState = menuStates.InGame;

        GameManager.Instance.PlayShort();

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
        EndPanel.SetActive(false);

        GameManager.Instance.ToMenu();
    }

    public void Pause()
    {
        currentState = menuStates.Pause;
        PausePanel.SetActive(true);

    }

    public void SkillTree()
    {
        currentState = menuStates.SkillTree;
        MainMenuPanel.SetActive(false);
        SkillTreePanel.SetActive(true);
        GameManager.Instance.SkillTree();
        moneyText.text = GameManager.Instance.money.ToString();
    }

    public void Credits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void Settings()
    {
        currentState = menuStates.Settings;
        SettingsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void MainMenuBack()
    {
        currentState = menuStates.MainMenu;
        MainMenuPanel.SetActive(true);
        SkillTreePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        GameManager.Instance.MainMenuBack();
    }

    public void EndGame()
    {
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Max Combo : " + GameManager.Instance.maxCombo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        if(GameManager.Instance.shortGame)
            endDpsText.text = "DPS : " + (GameManager.Instance.score / 60).ToString();
        else
            endDpsText.text = "DPS : " + (GameManager.Instance.score / 360).ToString();
    }

    public void WinGame()
    {
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
    }

    public void ResetVariables()
    {
        dps.ResetLights();
    }

    public void MasterSlider()
    {
        SoundManager.Instance.ModifyAllVolume(masterSlider.value);
    }
    public void MusicsSlider()
    {
        SoundManager.Instance.ModifyMusicVolume(musicsSlider.value);
    }
    public void EffectsSlider()
    {
        SoundManager.Instance.ModifySFXVolume(effectsSlider.value);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
        Debug.Log("quit");
    }

}
