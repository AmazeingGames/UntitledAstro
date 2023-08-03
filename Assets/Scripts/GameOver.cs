using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    public bool HasGameEnded { get; private set; } = false;

    [SerializeField] GameObject GameOverCanvas;

    public Action EndGame;
    public event Action GameOverEvent;

    private void Awake()
    {
        EndGame += OnGameOver;
    }

    void OnGameOver()
    {
        if (HasGameEnded == true) 
            return;

        HasGameEnded = true;
        GameOverCanvas.SetActive(true);

        GameOverEvent.Invoke();
    }
}
