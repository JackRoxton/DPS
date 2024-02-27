using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public PlayerInput inputs;
    public Player player;

    private void Awake()
    {
        inputs = GetComponent<PlayerInput>();

        DPSInputs inputActions = new DPSInputs();
        inputActions.Player.Enable();
        inputActions.Player.Dodge.performed += Dodge;
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            player.Dodge();
        }
    }
}
