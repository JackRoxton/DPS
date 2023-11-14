using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float Timer = 360f;
    public float altTimer = 15f;
    public float speedMod = 1f;

    public float score = 0, combo = 0;
    public float money = 0;

    public float playerAttack = 10;

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
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == gameStates.InGame) {
            Timer -= Time.deltaTime;
            altTimer -= Time.deltaTime;
            }

        if (Timer <= 0) Debug.Log("fin");

        if (altTimer <= 0)
        {
            ChangeRoom();   
        }
    }

    public void Play()
    {
        ResetVariables();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

        currentState = gameStates.InGame;
    }

    public void Resume()
    {
        Time.timeScale = 1;

        currentState = gameStates.InGame;
    }

    public void Pause()
    {
        Time.timeScale = 0;

        currentState = gameStates.Pause;
    }

    public void ToMenu()
    {
        Time.timeScale = 1;

        currentState = gameStates.MainMenu;
    }

    public void ResetVariables()
    {
        Timer = 360f;
        score = 0;
        combo = 0;
    }

    public void AddScore()
    {
        combo++;
        score += playerAttack * combo;
    }

    public void DPSCycle()
    {
        combo += 5;
        score += playerAttack * combo;
    }

    public void TakeDamage()
    {
        Debug.Log("a");
        combo = 0;
    }

    public void ChangeRoom()
    {
        speedMod *= 1.05f;
        altTimer = 30f;
        RoomManager.Instance.ChangeRoom();
    }

    public void Quit()
    {
        Application.Quit(); // ajouter sauvegarde
    }

}
