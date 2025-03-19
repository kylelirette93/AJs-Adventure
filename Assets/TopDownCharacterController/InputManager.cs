using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour, GameInput.IPlayerActions
{
    GameInput gameInput;
    void Start()
    {
        // Initialize the instance of game input.
        gameInput = new GameInput();

        // Enable the player action map.
        gameInput.Player.Enable();

        // Set the callbacks for the player action map.
        gameInput.Player.SetCallbacks(this);
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        // Invoke the move event with value of the context.
        Actions.MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Should be running.");
            Actions.ShiftKeyPressed?.Invoke();
        }
        if (context.canceled)
        {
            Debug.Log("Should stop running.");
            Actions.ShiftKeyReleased?.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Invoke the start interact event, based on started context.
            Actions.StartInteractEvent?.Invoke();
        }
        if (context.canceled)
        {
            // Invoke the canceled interact event, based on canceled context.
            Actions.CancelInteractEvent?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Dash performed");
            Actions.DashEvent?.Invoke();
        }
    }
}

public static class Actions
{
    // Define events for each action.
    public static Action<Vector2> MoveEvent;
    public static Action DashEvent;
    public static Action ShiftKeyPressed;
    public static Action ShiftKeyReleased;
    public static Action StartInteractEvent;
    public static Action CancelInteractEvent;
}