using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnTouch;
    private InputActions inputActions;

    private void Awake() {
        Instance = this;

        inputActions = new InputActions();

        inputActions.Touch.Enable();
        inputActions.Player.Enable();

        inputActions.Touch.Press.performed += Touch_Performed;
    }

    public Vector2 GetTouchPotitionVector(){
        Vector2 inputVector = inputActions.Touch.Press.ReadValue<Vector2>();
        
        return inputVector;
    }

    public Vector2 GetMovementVector(){
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        
        return inputVector;
    }

    private void Touch_Performed(InputAction.CallbackContext context){
        OnTouch?.Invoke(this, EventArgs.Empty);
    }
}
