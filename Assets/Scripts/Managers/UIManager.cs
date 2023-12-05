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
    public TMP_Text endDialogueText;
    public Image dialogueImageLeft, dialogueImageRight;
    public DPSCycle dps;

    public Dialogue Tuto, Win;
    Dialogue currentDialogue;

    public Fade fade;

    public Slider masterSlider, musicsSlider, effectsSlider;
    public Toggle dodgeToggle;

    public bool tutorial = true;
    bool tutoFlag = true;

    float fadeTime = 0.1f;
    bool fadeFlag = true;

    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,//
        InGame,
        Tutorial
    }
    public menuStates currentState = menuStates.MainMenu;
    menuStates previousMenu;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == menuStates.InGame || currentState == menuStates.Tutorial)
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

    IEnumerator FadeTime(int i)
    {
        yield return new WaitForSeconds(fadeTime);
        fadeFlag = false;

        switch(i){
            case 0: 
                Play();
                break;
            case 1:
                PlayShort();
                break;
            case 2:
                MainMenu();
                break;
        }
        fade.gameObject.SetActive(false);
    }

    public void Play()
    {
        if (fadeFlag)
        {
            GameManager.Instance.SetTimeScale(1);
            if (tutorial)
                Fade(2);
            else
                Fade(1);

            StartCoroutine(FadeTime(0));
            return;
        }

        if(tutorial)
        {
            //Fade(2);
            currentState = menuStates.Tutorial;
            DialoguePanel.SetActive(true);
            tutorial = false;
        }
        else
        {
            //Fade(1);
            currentState = menuStates.InGame;
            
        }
        

        GameManager.Instance.Play();

        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        fadeFlag = true;
    }

    public void PlayShort()
    {
        if (fadeFlag)
        {
            GameManager.Instance.SetTimeScale(1);
            if (tutorial)
                Fade(2);
            else
                Fade(1);

            StartCoroutine(FadeTime(1));
            return;
        }
        //Fade(1);
        if (tutorial)
        {
            //Fade(2);
            currentState = menuStates.Tutorial;
            DialoguePanel.SetActive(true);
            tutorial = false;
        }
        else
        {
            //Fade(1);
            currentState = menuStates.InGame;
            
        }

        GameManager.Instance.PlayShort();

        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        fadeFlag = true;
    }

    public void Resume()
    {
        RoomManager.Instance.Pause(false);
        InGamePanel.SetActive(true);
        currentState = menuStates.InGame;

        GameManager.Instance.Resume();

        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }


    public void MainMenu()
    {
        if (fadeFlag)
        {
            GameManager.Instance.SetTimeScale(1);
            Fade(0);

            StartCoroutine(FadeTime(2));
            return;
        }
        //Fade(0);



        currentState = menuStates.MainMenu;

        MainMenuPanel.SetActive(true);
        InGamePanel.SetActive(false);
        PausePanel.SetActive(false);
        EndPanel.SetActive(false);
        SkillTreePanel.SetActive(false);

        GameManager.Instance.ToMenu();

        fadeFlag = true;
    }

    public void Pause()
    {
        RoomManager.Instance.Pause(true);
        currentState = menuStates.Pause;
        PausePanel.SetActive(true);
        InGamePanel.SetActive(false);
    }

    public void Credits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void Settings()
    {
        previousMenu = currentState;
        currentState = menuStates.Settings;
        SettingsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    public void MainMenuBack()
    {
        if(previousMenu == menuStates.MainMenu)
        {
            currentState = menuStates.MainMenu;
            MainMenuPanel.SetActive(true);
            SkillTreePanel.SetActive(false);
            SettingsPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            GameManager.Instance.MainMenuBack();
        }
        if(previousMenu == menuStates.Pause)
        {
            currentState = menuStates.Pause;
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(true);
            GameManager.Instance.MainMenuBack();
        }
    }

    public void SkillTree()
    {
        currentState = menuStates.SkillTree;
        //MainMenuPanel.SetActive(false);
        EndPanel.SetActive(false);
        SkillTreePanel.SetActive(true);
        GameManager.Instance.SkillTree();
        moneyText.text = "Money : " + GameManager.Instance.money.ToString();
    }

    public void EndButton()
    {
        if (GameManager.Instance.winFlag) MainMenu();
        if(GameManager.Instance.phase > 0)
        {
            SkillTree();
            return;
        }
        MainMenu();
    }

    public void EndGame()
    {
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Max Combo : " + GameManager.Instance.maxCombo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        if(GameManager.Instance.winFlag)
            endDialogueText.text = "Game Over. Congratulations !";
        else
            endDialogueText.text = "Game Over. Try again !";
        if (GameManager.Instance.shortGame)
            endDpsText.text = "DPS : " + (GameManager.Instance.score / 60).ToString();
        else
            endDpsText.text = "Total DPS : " + (GameManager.Instance.score / 180).ToString();
    }


    public void EndPhase()
    {
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Max Combo : " + GameManager.Instance.maxCombo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        endDpsText.text = "DPS : " + (GameManager.Instance.score / 60).ToString();
        endDialogueText.text = "Remaining Phases : "+ (GameManager.Instance.phase);
    }

    public void NextPhase()
    {
        SkillTreePanel.SetActive(false);
        InGamePanel.SetActive(true);
        currentState = menuStates.InGame;
        GameManager.Instance.NextPhase();
    }

    public void WinGame()
    {
        WinDialogue();
        InGamePanel.SetActive(false);
        //EndPanel.SetActive(true);
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
        DialoguePanel.SetActive(true);
        /*Dialogue tmp = new Dialogue();
        tmp = Tuto;*/
        DialogueManager.Instance.StartDialogue(Tuto);
        currentDialogue = Tuto;
    }

    public void WinDialogue()
    {
        DialoguePanel.SetActive(true);
        DialogueManager.Instance.StartDialogue(Win);
        currentDialogue = Win;
    }

    public void NextDialogue()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }

    public void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        if(currentDialogue == Tuto)
        {
            if(GameManager.Instance.shortGame)
                PlayShort();
            else
                Play();
        }
        else if(currentDialogue == Win)
        {
            EndGame();
        }
    }

    public void Fade(int i)
    {
        fade.gameObject.SetActive(true);
        fade.FadeOut(i);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
        Debug.Log("quit");
    }

    public void DodgeControlCheck()
    {
        GameManager.Instance.playerDashOnMovement = dodgeToggle.isOn;
    }

    public void ReTuto()
    {
        tutorial = true;
        GameManager.Instance.tutorial = true;
    }

}
