using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    public UnityEvent MainMenuOn;
    public UnityEvent MainMenuOff;
    private bool isPaused;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            
            if (isPaused)
            {
                Time.timeScale = 0;
                GamePaused.Invoke();
                
            }
            else
            {
                Time.timeScale = 1;
                GameResumed.Invoke();
                
            }
        }
    }
}
