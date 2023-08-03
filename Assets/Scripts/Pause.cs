using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    [SerializeField] GameObject creditsCanvas;
    
    GameOver gameOver;
    GameObject mainMenuCanvas;
    
    public bool IsPaused { get; private set; }
    public bool IsGameRunning { get; private set; } = false;
    public bool IsGameOver { get; private set; } = false;

    void Start()
    {
        mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        gameOver = GetComponent<GameOver>();

        gameOver.GameOverEvent += HandleGameOver;
    }


    // Update is called once per frame
    void Update()
    {
        if (!mainMenuCanvas.activeInHierarchy && !creditsCanvas.activeInHierarchy && !IsGameOver)
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

    void HandleGameOver()
    {
        Debug.Log("Game Over handled");
        IsGameRunning = false;
        IsPaused = true;
        IsGameOver = true;
    }
}