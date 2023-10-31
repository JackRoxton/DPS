using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton
{
    public GameObject MainMenuCanvas, SkillTreeCanvas;
    public GameObject InGameCanvas, PauseCanvas;
    public GameObject SettingsCanvas;

    //+ panels je suppose

    public List<GameObject> existingObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //selon la scene spawn 1e bon canvas?
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.A)) {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            MainMenu();
        }*/
    }

    public void Play()
    {
        foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(InGameCanvas));
    }

    public void MainMenu()
    {
        foreach (GameObject obj in existingObjects){
            Destroy(obj);
        }
        existingObjects.Clear();
        existingObjects.Add(Instantiate(MainMenuCanvas));
    }

}
