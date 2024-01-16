using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject MainMenuPanel, SkillTreePanel;
    public GameObject InGamePanel, PausePanel, EndPanel, CreditsPanel;
    public GameObject CreditsButton;
    public GameObject SettingsPanel, DialoguePanel;

    public GameObject FloatingTextPrefab;
    GameObject currentText;

    public TMP_Text timerText, scoreText;
    public TMP_Text endComboText, endScoreText, endDpsText;
    public TMP_Text moneyText;
    public TMP_Text dialogueText, nameText;
    public TMP_Text endDialogueText;
    public Image dialogueImageLeft, dialogueImageRight;
    public LifeBar lifebar;
    public DPSCycle dps;

    public Dialogue Tuto, Win;
    public Dialogue End;
    Dialogue currentDialogue;

    public Fade fade;

    public Slider masterSlider, musicsSlider, effectsSlider;
    public Toggle dodgeToggle;

    public bool tutorial = true;
    bool tutoFlag = true;

    float fadeTime = 0.1f;
    bool fadeFlag = true;

    bool dialFlag = true;

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
        if (PlayerPrefs.GetInt("Tuto") == 1)
        {
            tutorial = true;
            tutoFlag = true;
        }
        else
        {
            tutorial = false;
            tutoFlag = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == menuStates.InGame || currentState == menuStates.Tutorial)
        {
            timerText.text = ((int)GameManager.Instance.globalTimer).ToString();
            scoreText.text = GameManager.Instance.score.ToString();
        }

        if(currentState == menuStates.Tutorial && tutoFlag) 
        {
            TutoDialogue();
            tutoFlag = false;
        }

        /*(Input.GetKeyDown(KeyCode.G))
        {
            GameManager.Instance.Cutscene();
        }*/

    }

    IEnumerator FadeTime(int i)
    {
        yield return new WaitForSeconds(fadeTime*2);
        fadeFlag = false;

        switch(i){
            case 0: 
                Play();
                break;
            case 1:
                //PlayShort();
                break;
            case 2:
                MainMenu();
                break;
            case 3:
                break;
        }
        fade.gameObject.SetActive(false);
    }

    public void Play()
    {
        if (fadeFlag)
        {
            //GameManager.Instance.SetTimeScale(1);
            if (tutorial)
                FadeScene(2);
            else
                FadeScene(1);

            StartCoroutine(FadeTime(0));
            return;
        }

        if(tutorial)
        {
            //Fade(2);
            GameManager.Instance.Play();
            TutoDialogue();
            currentState = menuStates.Tutorial;
            DialoguePanel.SetActive(true);
            tutorial = false;
        }
        else
        {
            //Fade(1);
            GameManager.Instance.Play();
            currentState = menuStates.InGame;
            
        }
        

        //GameManager.Instance.Play();

        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        fadeFlag = true;
    }

    /*public void PlayShort()
    {
        if (fadeFlag)
        {
            //GameManager.Instance.SetTimeScale(1);
            if (tutorial)
                FadeScene(2);
            else
                FadeScene(1);

            StartCoroutine(FadeTime(1));
            return;
        }
        //Fade(1);
        if (tutorial)
        {
            //Fade(2);.
            GameManager.Instance.PlayShort();
            TutoDialogue();
            currentState = menuStates.Tutorial;
            DialoguePanel.SetActive(true);
            tutorial = false;
        }
        else
        {
            //Fade(1);
            GameManager.Instance.PlayShort();
            currentState = menuStates.InGame;
            
        }

        //GameManager.Instance.PlayShort();

        MainMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        fadeFlag = true;
    }*/

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
        if (GameManager.Instance.winFlag)
        {
            EndPanel.SetActive(false);
            InGamePanel.SetActive(false);
            FadeScene(3);
            FadeTime(3);
            //GameManager.Instance.Cutscene();
            return;
        }

        if (fadeFlag)
        {
            GameManager.Instance.SetTimeScale(1);
            FadeScene(0);

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

    public void Credits(bool cutscene = false, bool onoff = true)
    {
        if(cutscene)
        {
            CreditsPanel.SetActive(onoff);
            CreditsButton.SetActive(!onoff);
        }
        else
        {
            MainMenuPanel.SetActive(!onoff);
            CreditsButton.SetActive(onoff);
        }
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
        }
        else
        {
            MainMenu();
        }
    }

    public void EndPhaseDialogue()
    {
        DialoguePanel.SetActive(true);
        if (dialFlag)
        {
            currentDialogue = End;
            DialogueManager.Instance.StartDialogue(End);
            dialFlag = false;
        }
        else
        {
            NextDialogue();
        }
    }

    public void EndGame()
    {

        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Combo : " + GameManager.Instance.combo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        if(GameManager.Instance.winFlag)
            endDialogueText.text = "Game Over. Congratulations !";
        else
            endDialogueText.text = "Game Over. Try again !";
        /*if (GameManager.Instance.shortGame)
            endDpsText.text = "DPS : " + (GameManager.Instance.score / 60).ToString();
        else*/
        endDpsText.text = "Total DPS : " + (GameManager.Instance.score / 180).ToString();
    }


    public void EndPhase()
    {
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Combo : " + GameManager.Instance.combo.ToString();
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

    public void Resetvar()
    {
        dialFlag = true;
        dps.ResetLights();
        lifebar.Resetvar();
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
        DialoguePanel.SetActive(true);
        DialogueManager.Instance.DisplayNextSentence();
    }

    public void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        if(currentDialogue == Tuto)
        {
            Play();
        }
        else if(currentDialogue == Win)
        {
            EndGame();
        }
    }

    public void FadeScene(int i = -1)
    {
        fade.gameObject.SetActive(true);
        fade.FadeInOut(i);
    }

    public void FadeIn(bool a = true)
    {
        fade.gameObject.SetActive(true);
        if (a) fade.FadeIn();
        else fade.FadeOut();
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
        tutoFlag = true;
        GameManager.Instance.tutorial = true;
        PlayerPrefs.SetInt("Tuto", 1);
    }

    public void HPDown()
    {
        lifebar.HPDown();
    }

    public void FloatingText(Vector2 pos, string text = "", bool attached = false, GameObject go = null, Color32? c = null)
    {
        if(c == null)
        {
            c = Color.white;
        }
        if(text == "")
        {
            text = (GameManager.Instance.playerAttack+GameManager.Instance.combo).ToString();
            if(!currentText)
            {
                currentText = Instantiate(FloatingTextPrefab, pos, Quaternion.identity);
                currentText.transform.position += new Vector3(0, 1, 0);
                currentText.GetComponent<TextMeshPro>().text = text;
                //Debug.Log(c.Value);
                if (c != null)
                    currentText.GetComponent<TextMeshPro>().color = c.Value;
                currentText.GetComponent<FloatingText>().n = GameManager.Instance.playerAttack + GameManager.Instance.combo;
                if (attached) 
                    currentText.transform.SetParent(go.transform, true);

            }
            else
            {
                currentText.GetComponent<TextMeshPro>().text = (currentText.GetComponent<FloatingText>().n + GameManager.Instance.playerAttack + GameManager.Instance.combo).ToString();
                currentText.GetComponent<FloatingText>().n += GameManager.Instance.playerAttack + GameManager.Instance.combo;
            }
            currentText.GetComponent<FloatingText>().ResetTimer();
        }
        else
        {
            GameObject a = Instantiate(FloatingTextPrefab, pos, Quaternion.identity);
            a.GetComponent<TextMeshPro>().text = text;
            a.transform.position += new Vector3(0, 1, 0);
            //Debug.Log(c.Value);
            if (c != null)
                a.GetComponent<TextMeshPro>().color = c.Value;
            if (attached) a.transform.SetParent(go.transform,true);
        }
    }

}
