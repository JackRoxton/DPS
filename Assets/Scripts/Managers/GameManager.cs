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
    //public float speedMod = 1f;

    public float mageHP = 5000;
    public float score = 0, combo = 1;
    public float maxCombo = 0;
    public float money = 0;
    public bool endFlag = false;
    public bool winFlag = false;

    public float playerAttack = 10;
    public float playerSpeedUp = 1;
    public bool playerDashOnMovement = false;

    public bool shortGame = false;
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
        timer = altTimer;
        DontDestroyOnLoad(this.gameObject);
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
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

        if (Input.GetKeyDown(KeyCode.Backspace)) globalTimer = 1f;
    }

    public void Play()
    {
        ResetVariables();
        if(tutorial)
        {
            tutorial = false;
            currentState = gameStates.Tutorial;
            //SetTimeScale(1);
        }
        else
        {
            currentState = gameStates.InGame;
            //SetTimeScale(1);

            shortGame = false;

            currentMusic = Random.Range(0, 9) + 1;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
        }

    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void PlayShort()
    {
        ResetVariables();
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

            currentMusic = Random.Range(0, 9) + 1;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
        }
        globalTimer = 60f;
        phase = 1;
        //Time.timeScale = 1;

        shortGame = true;

    }

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

    public void ToMenu()
    {
        //SetTimeScale(1);
        winFlag = false;
        endFlag = false;
        score = 0;
        currentState = gameStates.MainMenu;
        ResetVariables();
        //UIManager.Instance.Fade(0);
        //SceneManager.LoadScene(0);
    }

    public void ResetVariables()
    {
        phase = 3;
        globalTimer = 60f;
        altTimer = 7.5f;
        timer = altTimer;
        score = 0;
        combo = 1;
        maxCombo = 0;
        endFlag = false;
        winFlag = false;

        foreach(Skill skill in skills)
        {
            skill.Resetvar();
        }

        playerAttack = 10;
        playerSpeedUp = 1;

        UIManager.Instance.ResetVariables();
    }

    public void AddScore()
    {
        //combo++;
        //if (combo > maxCombo) maxCombo = combo;

        score += playerAttack * combo;

        if(score >= mageHP)
        {
            WinGame();
        }
    }

    public void DPSCycle()
    {
        combo += 2;
        if (combo > maxCombo) maxCombo = combo;
        AddScore();
    }

    public void TakeDamage()
    {
        ScreenShake();
        combo = 1;
    }

    public void ChangeRoom()
    {
        //speedMod *= 1.05f;
        timer = altTimer;
        currentMusic = (Random.Range(0, 9) + 1);
        SoundManager.Instance.PlayMusic(currentMusic.ToString());
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
        if (shortGame) EndGame();
        money += score;

        SoundManager.Instance.StopMusic(currentMusic.ToString());

        phase -= 1;
        if (phase > 0)
        {
            RoomManager.Instance.EndGame();
            UIManager.Instance.EndPhase();
            return;
        }
        EndGame();
    }

    public void NextPhase()
    {
        endFlag = false;
        globalTimer = 60f;
        timer = altTimer;
        currentState = gameStates.InGame;
        RoomManager.Instance.NextPhase();
        currentMusic = (Random.Range(0, 9) + 1);
        SoundManager.Instance.PlayMusic(currentMusic.ToString());
    }

    public void WinGame()
    {
        Debug.Log("Game Won");
        //Time.timeScale = 0;
        endFlag = true;
        winFlag = true;
        UIManager.Instance.WinGame();
        RoomManager.Instance.EndGame();
    }

    public void SkillTree()
    {
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
        SpeedUp
    }

    public void BuySkill(Skills type, int cost)
    {
        money -= cost;
        UIManager.Instance.moneyText.text = "Money : " + money.ToString();

        switch (type)
        {
            case Skills.AttackUp:
                playerAttack += 5;
                break;

            case Skills.SpeedUp:
                playerSpeedUp += 0.1f;
                RoomManager.Instance.PlayerSpeed(playerSpeedUp);
                break;

        }
    }

    public void ScreenShake()
    {
        Camera.main.GetComponent<CameraScript>().ScreenShake();
    }

}
