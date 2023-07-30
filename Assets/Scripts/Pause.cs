using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    GameObject mainMenuCanvas;
    
    public bool IsPaused { get; private set; }
    public bool IsGameRunning { get; private set; } = false;

    void Start()
    {
        mainMenuCanvas = GameObject.Find("MainMenuCanvas");
    }


    // Update is called once per frame
    void Update()
    {
        if (!mainMenuCanvas.activeInHierarchy)
        {
            IsGameRunning = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsPaused = !IsPaused;

                if (IsPaused)
                {
                    IsGameRunning = false;
                    Time.timeScale = 0;
                    GamePaused.Invoke();
                }
                else
                {
                    IsGameRunning = true;
                    Time.timeScale = 1;
                    GameResumed.Invoke();
                }
            }
        }
        else
            IsGameRunning = false;
    }
}