using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject MainMenuPanel, SkillTreePanel;
    public GameObject InGamePanel, PausePanel, EndPanel, CreditsPanel;
    public GameObject SettingsPanel, DialoguePanel;

    public TMP_Text timerText, scoreText, comboText;
    public TMP_Text endComboText, endScoreText, endDpsText;
    public TMP_Text moneyText;
    public TMP_Text dialogueText, nameText;
    public Image dialogueImage;
    public DPSCycle dps;

    public Dialogue Tuto, End;

    public Fade fade;

    public Slider masterSlider, musicsSlider, effectsSlider;
    public bool tutorial = true;
    bool tutoFlag = true;


    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame,
        Tutorial
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

        if(currentState == menuStates.Tutorial && tutoFlag) 
        {
            TutoDialogue();
            tutoFlag = false;
        }
    }

    public void Play()
    {
        if(tutorial)
        {
            Fade(2);
            currentState = menuStates.Tutorial;
            DialoguePanel.SetActive(true);
            tutorial = false;
        }
        else
        {
            Fade(1);
            currentState = menuStates.InGame;
            InGamePanel.SetActive(true);
        }
        

        GameManager.Instance.Play();

        MainMenuPanel.SetActive(false);
        
    }

    public void PlayShort()
    {
        Fade(1);

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
        Fade(0);

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

    public bool DialogueIsActive()
    {
        return DialoguePanel.activeSelf;
    }

    public void TutoDialogue()
    {
        //DialogueManager.Instance.dialogueText = dialogueText;
        //DialogueManager.Instance.nameText = nameText;
        DialoguePanel.SetActive(true);
        DialogueManager.Instance.StartDialogue(Tuto);
    }

    public void NextDialogue()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }

    public void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        if(currentState == menuStates.Tutorial)
        {
            Play();
        }
    }

    public void Fade(int i)
    {
        fade.FadeOut(i);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
        Debug.Log("quit");
    }

}
