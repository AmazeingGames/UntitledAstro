using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
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

    void OnGameOver()
    {
        Debug.Log("end game");
        GameOverCanvas.SetActive(true);

        YourScoreValueText.text = $"Score: {keepingScore.Score}";

        GameOverEvent.Invoke();
    }
}
