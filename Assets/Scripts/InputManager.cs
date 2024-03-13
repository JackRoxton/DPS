using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public enum InputType
    {
        MouseKeyboard,
        Controller
    };

    private InputType InputState = InputType.MouseKeyboard;

    void Update()
    {
        switch (InputState)
        {
            case InputType.MouseKeyboard:
                if (isControllerInput())
                {
                    InputState = InputType.Controller;
                    PlayerPrefs.SetInt("Controller",1);
                    GameManager.Instance.SetControls(true);
                    Debug.Log("Switched Input to Controller");
                }
                break;
            case InputType.Controller:
                if (isMouseKeyboard())
                {
                    InputState = InputType.MouseKeyboard;
                    PlayerPrefs.SetInt("Controller", 0);
                    GameManager.Instance.SetControls(false);
                    Debug.Log("Switched Input to Mouse/Keyboard");
                }
                break;
        }
    }

    public InputType GetInputType()
    {
        return InputState;
    }

    public bool UsingController()
    {
        return InputState == InputType.Controller;
    }

    private bool isMouseKeyboard()
    {
        // mouse & keyboard buttons and mouse movement
        if (Input.GetKey(KeyCode.Z) ||
            Input.GetKey(KeyCode.Q) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2) ||
            Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool isControllerInput()
    {
        // joystick buttons
        // check if we're not using a key for the axis' at the end 
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
            Input.GetKey(KeyCode.Joystick1Button1) ||
            Input.GetKey(KeyCode.Joystick1Button2) ||
            Input.GetKey(KeyCode.Joystick1Button3) ||
            Input.GetKey(KeyCode.Joystick1Button4) ||
            Input.GetKey(KeyCode.Joystick1Button5) ||
            Input.GetKey(KeyCode.Joystick1Button6) ||
            Input.GetKey(KeyCode.Joystick1Button7) ||
            Input.GetKey(KeyCode.Joystick1Button8) ||
            Input.GetKey(KeyCode.Joystick1Button9) ||
            Input.GetKey(KeyCode.Joystick1Button10) ||
            Input.GetKey(KeyCode.Joystick1Button11) ||
            Input.GetKey(KeyCode.Joystick1Button12) ||
            Input.GetKey(KeyCode.Joystick1Button13) ||
            Input.GetKey(KeyCode.Joystick1Button14) ||
            Input.GetKey(KeyCode.Joystick1Button15) ||
            Input.GetKey(KeyCode.Joystick1Button16) ||
            Input.GetKey(KeyCode.Joystick1Button17) ||
            Input.GetKey(KeyCode.Joystick1Button18) ||
            Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }

        return false;
    }
}
