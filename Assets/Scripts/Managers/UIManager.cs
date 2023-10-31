using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class UIManager : Singleton
{
    public GameObject MainMenuCanvas, SkillTreeCanvas;
    public GameObject InGameCanvas, PauseCanvas;
    public GameObject SettingsCanvas;

    //+ panels je suppose

    public float Timer = 5f;

    public List<GameObject> existingObjects = new List<GameObject>();

    public enum menuStates
    {
        MainMenu,
        SkillTree,
        Pause,
        Settings,
        InGame
    }
    public menuStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        //selon la scene spawn 1e bon canvas?
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(InGameCanvas));
        currentState = menuStates.InGame;
    }

    public void MainMenu()
    {
        foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(MainMenuCanvas));
        currentState = menuStates.MainMenu;
    }

    public void SkillTree()
    {
        foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(SkillTreeCanvas));
        currentState = menuStates.SkillTree;
    }

    public void PauseMenu()
    {
        existingObjects.Add(Instantiate(PauseCanvas));
        currentState = menuStates.Pause;
    }

    public void Settings()
    {
        existingObjects.Add(Instantiate(PauseCanvas));
        currentState = menuStates.Settings;
    }

}
