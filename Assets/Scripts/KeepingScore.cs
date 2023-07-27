using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepingScore : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if "player" moves forward by 1 unit
        
        UpdateScore(1); //add 1 score to score.
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd; 
        scoreText.text = "0" + score;
    }
}
