using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("Event System First")]
    public GameObject MainMenuFirst, SettingsFirst, PauseFirst, CreditsFirst, EndFirst/*, SkillTreeFirst*/;

    [Header("Panels")]
    public GameObject MainMenuPanel/*, SkillTreePanel*/;
    public GameObject InGamePanel, PausePanel, EndPanel, CreditsPanel;
    public GameObject CreditsMenuButton, CreditsButton, creditsText;
    public GameObject SettingsPanel, DialoguePanel;

    [Header("In Game")]
    public TMP_Text timerText, scoreText;
    public Image timer;
    public TMP_Text endComboText, endScoreText, endDpsText;
    //public TMP_Text moneyText;
    public TMP_Text dialogueText, nameText;
    public TMP_Text endDialogueText;
    //public TMP_Text bonusText;
    public Image dialogueImageLeft, dialogueImageRight;
    public LifeBar lifebar;
    public DPSCycle dps;

    [Header("Dialogues")]
    public Dialogue Tuto, Win;
    public Dialogue End;
    Dialogue currentDialogue;

    [Header("Sound")]
    public Slider masterSlider, musicsSlider, effectsSlider;
    public Toggle dodgeToggle, mouseToggle;

    [Header("Tutorial")]
    public bool tutorial = true;
    bool tutoFlag = true;
    public TMP_Text controlsText;
    public TMP_Text shownText;
    public Image controlsImage;
    public Sprite key, mouse1, mouse2, button, bumper;
    string controlCheck = "";

    public GameObject FloatingTextPrefab;
    GameObject currentText;
    public Fade fade;
    float fadeTime = 0.1f;
    bool fadeFlag = true;

    public DialogueClick dialogueClick;

    bool dialFlag = true;

    public enum menuStates
    {
        MainMenu,
        //SkillTree,
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
        if (!PlayerPrefs.HasKey("Tuto"))
            PlayerPrefs.SetInt("Tuto", 1);
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

        //bonusText.gameObject.SetActive(false);
        CreditsMenuButton.GetComponent<Button>().onClick.AddListener(_Credits);


        if (!PlayerPrefs.HasKey("Master"))
            PlayerPrefs.SetFloat("Master", 1);
        else
            SoundManager.Instance.masterVolume = PlayerPrefs.GetInt("Master");

        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 1);
        else
            SoundManager.Instance.sfxVolume = PlayerPrefs.GetInt("SFX");

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 1);
        else
            SoundManager.Instance.musicVolume = PlayerPrefs.GetInt("Music");

        if (!PlayerPrefs.HasKey("DodgeOnM"))
            PlayerPrefs.SetInt("DodgeOnM", 0);
        else
        {
            if (PlayerPrefs.GetInt("DodgeOnM") == 1)
                dodgeToggle.isOn = true;
            else
                dodgeToggle.isOn = false;
            DodgeControlCheck();
        }

        if(!PlayerPrefs.HasKey("Controller"))
            PlayerPrefs.SetInt("Controller", 0);
        else
        {
            if (PlayerPrefs.GetInt("Controller") == 1)
                mouseToggle.isOn = true;
            else
                mouseToggle.isOn = false;
            DodgeControlCheck();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == menuStates.InGame || currentState == menuStates.Tutorial)
        {
            timer.fillAmount = GameManager.Instance.globalTimer / 60;
            timerText.text = ((int)GameManager.Instance.globalTimer).ToString();
            scoreText.text = GameManager.Instance.score.ToString();
        }

        if(currentState == menuStates.Tutorial && tutoFlag) 
        {
            TutoDialogue();
            tutoFlag = false;
        }

        if(currentState == menuStates.Tutorial && controlCheck != "")
        {
            TutoControlCheck();
        }

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

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(null);

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
            //SkillTreePanel.SetActive(false);
            GameManager.Instance.Cutscene();

            FadeScene(3);
            StartCoroutine(FadeTime(3));

            EndPanel.SetActive(false);
            InGamePanel.SetActive(false);
            //SkillTreePanel.SetActive(false);
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
        //SkillTreePanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(MainMenuFirst);

        GameManager.Instance.ToMenu();

        fadeFlag = true;
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(PauseFirst);
        RoomManager.Instance.Pause(true);
        currentState = menuStates.Pause;
        PausePanel.SetActive(true);
        InGamePanel.SetActive(false);
    }

    void _Credits()
    {
        Credits();
    }
    public void Credits(bool cutscene = false, bool onoff = true)
    {
        if(cutscene)
        {
            CreditsPanel.SetActive(onoff);
            CreditsButton.SetActive(!onoff);
            creditsText.SetActive(onoff);
        }
        else
        {
            if(onoff)
                EventSystem.current.SetSelectedGameObject(CreditsFirst);
            else
                EventSystem.current.SetSelectedGameObject(MainMenuFirst);
            MainMenuPanel.SetActive(!onoff);
            CreditsPanel.SetActive(onoff);
            CreditsButton.SetActive(onoff);
            creditsText.SetActive(!onoff);
        }
    }

    public void Settings()
    {
        EventSystem.current.SetSelectedGameObject(SettingsFirst);
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
            EventSystem.current.SetSelectedGameObject(MainMenuFirst);
            currentState = menuStates.MainMenu;
            MainMenuPanel.SetActive(true);
            //SkillTreePanel.SetActive(false);
            SettingsPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            GameManager.Instance.MainMenuBack();
        }
        if(previousMenu == menuStates.Pause)
        {
            EventSystem.current.SetSelectedGameObject(PauseFirst);
            currentState = menuStates.Pause;
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(true);
            GameManager.Instance.MainMenuBack();
        }
    }

    /*public void SkillTree()
    {
        EventSystem.current.SetSelectedGameObject(SkillTreeFirst);
        currentState = menuStates.SkillTree;
        //MainMenuPanel.SetActive(false);

        if(GameManager.Instance.phase == 1 && GameManager.Instance.score < 3000)
        {
            bonusText.gameObject.SetActive(true);
            float x = (3000 - GameManager.Instance.score) / 2 + 500;
            bonusText.text = "You got "+x+" bonus money !";
            bonusText.GetComponent<Animator>().Play("BlinkText", 0);
            GameManager.Instance.money += x;
        }

        EndPanel.SetActive(false);
        SkillTreePanel.SetActive(true);
        GameManager.Instance.SkillTree();
        moneyText.text = "Money : " + GameManager.Instance.money.ToString();
    }*/

    public void EndButton()
    {
        if (GameManager.Instance.winFlag) MainMenu();
        if(GameManager.Instance.phase > 0)
        {
            EndPanel.SetActive(false);
            NextPhase();
            //SkillTree();
        }
        else
        {
            MainMenu();
        }
    }

    public void EndPhaseDialogue()
    {
        DialoguePanel.SetActive(true);
        dialogueClick.LockTimer();
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
        EventSystem.current.SetSelectedGameObject(EndFirst);
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Combo : " + GameManager.Instance.combo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        if(GameManager.Instance.winFlag)
            endDialogueText.text = "Game Over. Congratulations !";
        else
            endDialogueText.text = "Game Over. Try again !";

        endDpsText.text = "Total DPS : " + (GameManager.Instance.score / 180).ToString();
    }


    public void EndPhase()
    {
        EventSystem.current.SetSelectedGameObject(EndFirst);
        InGamePanel.SetActive(false);
        EndPanel.SetActive(true);
        endComboText.text = "Combo : " + GameManager.Instance.combo.ToString();
        endScoreText.text = "Score : " + GameManager.Instance.score.ToString();
        endDpsText.text = "DPS : " + (GameManager.Instance.score / 60).ToString();
        endDialogueText.text = "Remaining Phases : "+ (GameManager.Instance.phase);
    }

    public void NextPhase()
    {
        //SkillTreePanel.SetActive(false);
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
        PlayerPrefs.SetFloat("Master",masterSlider.value);
    }
    public void MusicsSlider()
    {
        SoundManager.Instance.ModifyMusicVolume(musicsSlider.value);
        PlayerPrefs.SetFloat("Music", masterSlider.value);
    }
    public void EffectsSlider()
    {
        SoundManager.Instance.ModifySFXVolume(effectsSlider.value);
        PlayerPrefs.SetFloat("SFX", masterSlider.value);
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

    public void SetControls()
    {
        GameManager.Instance.SetControls(!mouseToggle.isOn);

        if (mouseToggle.isOn)
            PlayerPrefs.SetInt("Controller", 1);
        else
            PlayerPrefs.SetInt("Controller", 0);
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
        
        if(dodgeToggle.isOn)
            PlayerPrefs.SetInt("DodgeOnM", 1);
        else
            PlayerPrefs.SetInt("DodgeOnM", 0);
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
    public void HPBump()
    {
        lifebar.HPBump();
    }

    public void FloatingText(Vector2 pos, string text = "", bool attached = false, GameObject go = null, Color32? c = null, float size = 6)
    {
        if(c == null)
        {
            c = Color.white;
        }
        if(text == "")
        {
            text = (GameManager.Instance.playerAttack+GameManager.Instance.combo).ToString();
            if (!currentText)
            {
                currentText = Instantiate(FloatingTextPrefab, pos, Quaternion.identity);
                currentText.GetComponent<TextMeshPro>().fontSize = size;
                currentText.transform.position += new Vector3(0, 1, 0);
                currentText.GetComponent<TextMeshPro>().text = text;
                //Debug.Log(c.Value);
                if (c != null)
                    currentText.GetComponent<TextMeshPro>().color = c.Value;
                currentText.GetComponent<FloatingText>().n = GameManager.Instance.playerAttack + GameManager.Instance.combo;
                if (attached)
                {
                    currentText.transform.SetParent(go.transform, true);
                }

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
            a.GetComponent<TextMeshPro>().fontSize = size;
            a.GetComponent<TextMeshPro>().text = text;
            a.transform.position += new Vector3(0, 1, 0);
            //Debug.Log(c.Value);
            if (c != null)
                a.GetComponent<TextMeshPro>().color = c.Value;
            if (attached) a.transform.SetParent(go.transform,true);
        }
    }

    public void ScoreBump()
    {
        scoreText.GetComponent<Animator>().Play("ScoreVibe");
    }
    public void DpsScoreBump()
    {
        scoreText.GetComponent<Animator>().Play("ScoreBump");
    }

    public void SetControlText(string text)
    {
        controlsText.gameObject.SetActive(true);
        controlsText.text = "Press      To " + text;

        controlCheck = text;

        switch (controlCheck)
        {
            case "Slash":
                if (PlayerPrefs.GetInt("Controller") == 0)
                {
                    controlsImage.sprite = mouse1;
                    shownText.gameObject.SetActive(false);
                }
                else
                {
                    controlsImage.sprite = bumper;
                    shownText.gameObject.SetActive(true);
                    shownText.text = "RB";
                }
                break;
            case "Dodge":
                if (PlayerPrefs.GetInt("Controller") == 0)
                {
                    controlsImage.sprite = key;
                    shownText.gameObject.SetActive(true);
                    shownText.text = "Space";
                }
                else
                {
                    controlsImage.sprite = bumper;
                    shownText.gameObject.SetActive(true);
                    shownText.text = "LB";
                }
                break;
            case "Parry":
                if (PlayerPrefs.GetInt("Controller") == 0)
                {
                    controlsImage.sprite = mouse2;
                    shownText.gameObject.SetActive(false);
                }
                else
                {
                    controlsImage.sprite = button;
                    shownText.gameObject.SetActive(true);
                    shownText.text = "X";
                }
                break;
        }
    }

    void TutoControlCheck()
    {
        switch(controlCheck)
        {
            case "Slash":
                if (Input.GetAxis("Slash") != 0)
                {
                    controlsText.gameObject.SetActive(false);
                    controlCheck = "";
                }
                break;
            case "Dodge":
                if (Input.GetAxis("Dodge") != 0)
                {
                    controlsText.gameObject.SetActive(false);
                    controlCheck = "";
                }
                break;
            case "Parry":
                if (Input.GetAxis("Parry") != 0)
                {
                    controlsText.gameObject.SetActive(false);
                    controlCheck = "";
                }
                break;
        }
    }

}
