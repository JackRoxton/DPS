using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float globalTimer = 360f;
    public float altTimer = 7.5f;
    float timer;
    bool tpeffectFlag = false;
    //public float speedMod = 1f;

    public float mageHP = 10000;
    public float score = 0, combo = 0;
    public float maxCombo = 0;
    public float money = 0;
    bool endFlag = false;

    public float playerAttack = 10;
    public float playerSpeedUp = 1;
    public bool playerDashOnMovement = false;

    public bool shortGame = false;
    public bool tutorial = true;

    int currentMusic = 0;

    public enum gameStates // peut-être certains à enlever
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,//
        InGame,
        Tutorial
    }
    public gameStates currentState = gameStates.MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        timer = altTimer;
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if(endFlag) return;



        if (currentState == gameStates.InGame) {
            globalTimer -= Time.deltaTime;
            timer -= Time.deltaTime;
        }

        if (globalTimer <= 0) EndGame();

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
    }

    public void Play()
    {
        ResetVariables();
        if(tutorial)
        {
            tutorial = false;
            currentState = gameStates.Tutorial;
            Time.timeScale = 1;
        }
        else
        {
            currentState = gameStates.InGame;
            Time.timeScale = 1;

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
            Time.timeScale = 1;
        }
        else
        {
            currentState = gameStates.InGame;
            Time.timeScale = 1;

            //shortGame = true;

            currentMusic = Random.Range(0, 9) + 1;
            SoundManager.Instance.PlayMusic(currentMusic.ToString());
            //shortGame = true;
        }
        globalTimer = 60f;
        //Time.timeScale = 1;

        shortGame = true;

    }

    public void Resume()
    {
        SoundManager.Instance.UnpauseMusic(currentMusic.ToString());
        Time.timeScale = 1;
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
            //Time.timeScale = 0;
            currentState = gameStates.Pause;
            UIManager.Instance.Pause();
            RoomManager.Instance.Pause(true);
        }
    }
    
    public void SkillTree()
    {
        currentState = gameStates.SkillTree;
    }

    public void MainMenuBack()
    {
        currentState = gameStates.MainMenu;
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        money += score;
        score = 0;
        currentState = gameStates.MainMenu;
        //UIManager.Instance.Fade(0);
        //SceneManager.LoadScene(0);
    }

    public void ResetVariables()
    {
        globalTimer = 360f;
        score = 0;
        combo = 0;
        maxCombo = 0;
        timer = altTimer;
        endFlag = false;
        UIManager.Instance.ResetVariables();
    }

    public void AddScore()
    {
        combo++;
        if (combo > maxCombo) maxCombo = combo;

        score += playerAttack * combo;

        if(score >= mageHP)
        {
            WinGame();
        }
    }

    public void DPSCycle()
    {
        combo += 5;
        AddScore();
    }

    public void TakeDamage()
    {
        ScreenShake();
        combo = 0;
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
        //money += score;
        endFlag = true;
        UIManager.Instance.EndGame();
        RoomManager.Instance.EndGame();
        
    }

    public void WinGame()
    {
        Debug.Log("Game Won");
        //Time.timeScale = 0;
        endFlag = true;
        UIManager.Instance.WinGame();
        RoomManager.Instance.WinGame();
    }

    public bool Affordable(int cost)
    {
        UIManager.Instance.moneyText.text = money.ToString();
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
        UIManager.Instance.moneyText.text = money.ToString();

        switch (type)
        {
            case Skills.AttackUp:
                playerAttack += 5;
                break;

            case Skills.SpeedUp:
                playerSpeedUp += 0.1f;
                break;

        }
    }

    public void ScreenShake()
    {
        Camera.main.GetComponent<CameraScript>().ScreenShake();
    }

}
