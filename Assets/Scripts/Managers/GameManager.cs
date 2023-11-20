using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float globalTimer = 360f;
    public float altTimer = 7.5f;
    float timer;
    //public float speedMod = 1f;

    public float score = 0, combo = 0;
    public float maxCombo = 0;
    public float money = 0;
    bool endFlag = false;

    public float playerAttack = 10;
    public float playerSpeedUp = 1;

    public enum gameStates // peut-être certains à enlever
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame
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

        if (timer <= 0 )
        {
            ChangeRoom();   
        }
    }

    public void Play()
    {
        ResetVariables();
        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        currentState = gameStates.InGame;
        SoundManager.Instance.PlayMusic((Random.Range(0, 9) + 1).ToString());
    }


    public void PlayShort()
    {
        ResetVariables();
        globalTimer = 60f;
        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        currentState = gameStates.InGame;
        SoundManager.Instance.PlayMusic((Random.Range(0, 9) + 1).ToString());

    }

    public void Resume()
    {
        Time.timeScale = 1;
        RoomManager.Instance.Pause(false);
        currentState = gameStates.InGame;
    }

    public void Pause()
    {
        Debug.Log(currentState);
        if(currentState == gameStates.Pause)
        {
            //Time.timeScale = 1;
            //currentState = gameStates.InGame;
            UIManager.Instance.Resume();
            //RoomManager.Instance.Pause(false);
        }
        else if (currentState == gameStates.InGame)
        {
            Time.timeScale = 0;
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
        SceneManager.LoadScene(0);
    }

    public void ResetVariables()
    {
        globalTimer = 360f;
        score = 0;
        combo = 0;
        maxCombo = 0;
        timer = altTimer;
        endFlag = false;
    }

    public void AddScore()
    {
        combo++;
        if (combo > maxCombo) maxCombo = combo;
        score += playerAttack * combo;
    }

    public void DPSCycle()
    {
        combo += 5;
        if(combo > maxCombo) maxCombo = combo;
        score += playerAttack * combo;
    }

    public void TakeDamage()
    {
        Debug.Log("a");
        combo = 0;
    }

    public void ChangeRoom()
    {
        //speedMod *= 1.05f;
        timer = altTimer;
        SoundManager.Instance.PlayMusic((Random.Range(0,9)+1).ToString());
        SoundManager.Instance.Play("change");
        RoomManager.Instance.ChangeRoom();

    }

    public void EndGame()
    {
        Time.timeScale = 0;
        money += score;
        endFlag = true;
        UIManager.Instance.EndGame();
        RoomManager.Instance.EndGame();
        
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

}
