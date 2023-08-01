using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    public UnityEvent onCollision;
    
    [SerializeField] GameObject GameOverCanvas;
    [SerializeField] TextMeshProUGUI YourScoreValueText;

    public Action EndGame;
    public event Action GameOverEvent;

    KeepingScore keepingScore;

    private void Awake()
    {
        EndGame += OnGameOver;

        if (gameObject.GetComponent<KeepingScore>() == null)
        {
            Debug.Log("can't find score");
        }

        keepingScore = gameObject.GetComponent<KeepingScore>();
    }

    private void Update()
    {
        
    }

    void OnGameOver()
    {
        Debug.Log("end game");
        GameOverCanvas.SetActive(true);

        YourScoreValueText.text = $"Score: {keepingScore.Score}";

        GameOverEvent.Invoke();
    }
}
