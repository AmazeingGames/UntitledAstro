using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepingScore : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;
    public bool isGameActive;
    void Start()
    {
        isGameActive = true;
        score = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if player moves forward in z axis 1 unit
       
        {
            UpdateScore(1); //add 1 score to score.
        }
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd; 
        scoreText.text = "Score:" + score;
    }
}
