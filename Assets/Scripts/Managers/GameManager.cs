using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float globalTimer = 60f;
    public float altTimer = 7.5f;
    float timer;
    public int phase = 3;
    bool tpeffectFlag = false;
    public float speedMod = 1f;

    public float mageHP = 5000;
    float threshold = 4000;
    public float score = 0, combo = 0;
    //public float maxCombo = 0;
    public float money = 0;
    public bool endFlag = false;
    public bool winFlag = false;

    public float playerAttack = 20;
    public float playerAttackSpeed = 1;
    public float playerSpeedUp = 1;
    public bool playerDashOnMovement = false;

    //public bool shortGame = false;
    public bool tutorial = true;

    public Skill[] skills;

    int currentMusic = 0;

    public enum gameStates // peut-être certains à enlever
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame,
        Tutorial
    }
    public gameStates currentState = gameStates.MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        timer = altTimer;
        DontDestroyOnLoad(this.gameObject);
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        if (!PlayerPrefs.HasKey("Tuto")) 
            PlayerPrefs.SetInt("Tuto", 1);
        if (PlayerPrefs.GetInt("Tuto") == 1)
            tutorial = true;
        else
            tutorial = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(endFlag) return;

        if (currentState == gameStates.InGame) {
            globalTimer -= Time.deltaTime;
            timer -= Time.deltaTime;
        }

        if (globalTimer <= 0) EndPhase();

        if(timer <= 2 && !tpeffectFlag)
        {
            tpeffectFlag = true;
            TeleportEffect.Instance.Effect();
        }
        if (timer <= 0 )
        {
            ChangeRoom();
            tpeffectFlag = false;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) globalTimer = 0f;
        if (Input.GetKeyDown(KeyCode.Return)) for (int i = 0; i < 10; i++) AddScore() ;
    }

    public void Play()
    {
        Resetvar();
        if(tutorial)
        {
            tutorial = false;
            PlayerPrefs.SetInt("Tuto",0);
            currentState = gameStates.Tutorial;
            //SetTimeScale(1);
            currentMusic = 2;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
        }
        else
        {
            currentState = gameStates.InGame;
            //SetTimeScale(1);

            //shortGame = false;

            SoundManager.Instance.StopMusic(currentMusic.ToString());
            currentMusic = 1;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
        }

    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    /*public void PlayShort()
    {
        Resetvar();
        if (tutorial)
        {
            tutorial = false;
            currentState = gameStates.Tutorial;
            //SetTimeScale(1);
        }
        else
        {
            currentState = gameStates.InGame;
            //SetTimeScale(1);

            currentMusic = 1;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
        }
        globalTimer = 60f;
        phase = 1;
        //Time.timeScale = 1;

        shortGame = true;

    }*/

    public void SetTimeScale(int i)
    {
        Time.timeScale = i;
    }

    public void Resume()
    {
        SoundManager.Instance.UnpauseMusic(currentMusic.ToString());
        //SetTimeScale(1);
        RoomManager.Instance.Pause(false);
        currentState = gameStates.InGame;
    }

    public void Pause()
    {
        if(currentState == gameStates.Pause)
        {
            UIManager.Instance.Resume();
        }
        else if (currentState == gameStates.InGame)
        {
            SoundManager.Instance.PauseMusic(currentMusic.ToString());
            //SetTimeScale(0);
            currentState = gameStates.Pause;
            UIManager.Instance.Pause();
            RoomManager.Instance.Pause(true);
        }
    }
    

    public void MainMenuBack()
    {
        currentState = gameStates.MainMenu;
    }

    public void Cutscene()
    {
        SoundManager.Instance.StopMusic(currentMusic.ToString());
        winFlag = false;
        endFlag = false;
    }

    public void ToMenu()
    {
        //SetTimeScale(1);
        SoundManager.Instance.StopMusic(currentMusic.ToString());
        winFlag = false;
        endFlag = false;
        score = 0;
        currentState = gameStates.MainMenu;
        Resetvar();
        UIManager.Instance.FadeScene(0);
        if(RoomManager.Instance != null)
            RoomManager.Instance.Resetvar();
        SceneManager.LoadScene(0);
    }

    public void Resetvar()
    {
        phase = 3;
        globalTimer = 60f;
        altTimer = 7.5f;
        timer = altTimer;
        score = 0;
        combo = 0;
        //maxCombo = 0;
        endFlag = false;
        winFlag = false;
        threshold = 4000;
        speedMod = 1;

        foreach(Skill skill in skills)
        {
            skill.Resetvar();
        }

        playerAttack = 20;
        playerSpeedUp = 1;
        playerAttackSpeed = 1;

        UIManager.Instance.Resetvar();
    }

    public void AddScore()
    {
        //combo++;
        //if (combo > maxCombo) maxCombo = combo;

        score += playerAttack + combo;

        if(score >= mageHP)
        {
            WinGame();
        }

        if((mageHP - score) < threshold)
        {
            UIManager.Instance.FloatingText(RoomManager.Instance.mage.transform.position, "ouch", false, null, Color.magenta);
            UIManager.Instance.HPDown();
            SoundManager.Instance.Play("magehpdown");
            threshold -= 1000;
            speedMod *= 1.1f;
        }
    }

    public void DPSCycle()
    {
        combo += 5;
        //if (combo > maxCombo) maxCombo = combo;
        AddScore();
        if(currentState == gameStates.InGame)
            RoomManager.Instance.Shockwave();
    }

    public void TakeDamage()
    {
        StrongScreenShake();
        //combo = 1;
    }

    public void ChangeRoom()
    {
        timer = altTimer / speedMod;
        //currentMusic = 1;
        //SoundManager.Instance.PlayMusic(currentMusic.ToString());
        SoundManager.Instance.Play("change");
        RoomManager.Instance.ChangeRoom();

    }

    public void EndGame()
    {
        //Time.timeScale = 0;
        UIManager.Instance.EndGame();
        if(RoomManager.Instance != null )
            RoomManager.Instance.EndGame();
        
    }

    public void EndPhase()
    {
        endFlag = true;
        SoundManager.Instance.StopMusic(currentMusic.ToString());
        phase -= 1;
        UIManager.Instance.EndPhaseDialogue();
        RoomManager.Instance.EndGame();

        /*if (phase > 0)
        {
            UIManager.Instance.EndPhaseDialogue();
            RoomManager.Instance.EndGame();
            //UIManager.Instance.EndPhase();
            //return;
        }
        else
        {
            UIManager.Instance.EndPhaseDialogue();
        }*/

        /*if (shortGame) EndGame();
        money += score;
        EndGame();*/
    }

    public void NextPhase()
    {
        endFlag = false;
        globalTimer = 60f;
        timer = altTimer;
        currentState = gameStates.InGame;
        RoomManager.Instance.NextPhase();
        currentMusic = 1;
        SoundManager.Instance.PlayMusic(currentMusic.ToString());
    }

    public void WinGame()
    {
        //Debug.Log("Game Won");
        //Time.timeScale = 0;
        endFlag = true;
        winFlag = true;
        UIManager.Instance.WinGame();
        RoomManager.Instance.EndGame();
    }

    /*public void Cutscene()
    {
        SceneManager.LoadScene(3);
    }*/

    public void SkillTree()
    {
        money += score;
        currentState = gameStates.SkillTree;
    }

    public bool Affordable(int cost)
    {
        UIManager.Instance.moneyText.text = "Money : " + money.ToString();
        if (money - cost >= 0) return true;
        else return false;
    }

    public void Quit()
    {
        Application.Quit(); // ajouter sauvegarde
    }

    public enum Skills
    {
        AttackUp,
        SpeedUp,
        AttackSpeedUp,
        DodgePow,
        ParryPow
    }

    public void BuySkill(Skills type, int cost)
    {
        money -= cost;
        UIManager.Instance.moneyText.text = "Money : " + money.ToString();

        switch (type)
        {
            case Skills.AttackUp:
                playerAttack += 20;
                RoomManager.Instance.PlayerAttack();
                break;

            case Skills.SpeedUp:
                playerSpeedUp *= 1.1f;
                RoomManager.Instance.PlayerSpeed(playerSpeedUp);
                break;

            case Skills.AttackSpeedUp:
                playerAttackSpeed *= 1.15f;
                RoomManager.Instance.PlayerAttackSpeed(playerAttackSpeed);
                break;

            case Skills.DodgePow:
                RoomManager.Instance.PlayerDodgePow(2);
                break;

            case Skills.ParryPow:
                RoomManager.Instance.PlayerParryPow(2);
                break;
        }
    }

    public void ScreenShake()
    {
        Camera.main.GetComponent<CameraScript>().ScreenShake();
    }

    public void StrongScreenShake()
    {
        Camera.main.GetComponent<CameraScript>().StrongScreenShake();
    }

}
