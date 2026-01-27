using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public static bool Paused;

    public Action OnPause;
    public Action OnUnpause;

    private PlayerInput input;
    private InputAction pauseAction;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        pauseAction = input.actions["Exit"];

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
