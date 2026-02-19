using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public static bool Paused;

    public Action OnPause;
    public Action OnUnpause;

    private InputSystem_Actions input;
    private InputAction pauseAction;

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Awake()
    {
        Instance = this;
        input = new();
    }

    private void Start()
    {
        pauseAction = input.UI.Pause;

        pauseAction.started += _ => TogglePause();
    }

    public static void TogglePause()
    {
        Paused = !Paused;
        if (Paused) 
        {
            Instance.OnPause?.Invoke();
            Time.timeScale = 0f;
        }
        else 
        {
            Instance.OnUnpause?.Invoke();
            Time.timeScale = 1f;
        }
    }
}
