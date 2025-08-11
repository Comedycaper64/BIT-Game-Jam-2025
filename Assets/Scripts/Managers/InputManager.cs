using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    private bool gameStarted = false;
    public static InputManager Instance { get; private set; }
    public static bool disableInputs = false;

    public Vector2 MovementValue { get; private set; }
    public Action OnAttackEvent;
    public Action OnDashEvent;
    public Action OnMenuEvent;

    private Controls controls;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        disableInputs = true;
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += ToggleDisableInputs;
        LightManager.OnLightExtinguished += GameEnd;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseGame -= ToggleDisableInputs;
        LightManager.OnLightExtinguished -= GameEnd;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MovementValue = Vector2.zero;
            return;
        }

        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (context.performed)
        {
            OnDashEvent?.Invoke();
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!gameStarted)
        {
            return;
        }

        if (context.performed)
        {
            OnMenuEvent?.Invoke();
        }
    }

    private void ToggleDisableInputs(object sender, bool toggle)
    {
        if (!gameStarted)
        {
            return;
        }

        disableInputs = toggle;
    }

    public void GameStart()
    {
        gameStarted = true;
        disableInputs = false;
    }

    public void GameEnd()
    {
        gameStarted = false;
        disableInputs = true;
    }
}
