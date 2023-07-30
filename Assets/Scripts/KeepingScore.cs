using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepingScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int score = 0;
    private int highscore = 0;
    void Start()
    {
        highscore = PlayerPrefs.GetInt("High Score:", highscore);
        highScoreText.text = highscore.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player moves forward in z axis 1 unit
       
        {
            UpdateScore(1); //add 1 score to score.
        }
        
        if(score > highscore)
        {
            highscore = score;
            highScoreText.text = "High Score:" + highscore;
        }
    }

    public void UpdateScore(int scoreToAdd) //score is added for every second player is alive.
    {
        score += scoreToAdd;
        scoreText.text = "Score:" + score;
    }

    void CheckHighScore() //if player beats highscore, remembers new highscore.
    { 
        if (score > PlayerPrefs.GetInt("High Score:", 0)) 
        {
            PlayerPrefs.SetInt("High Score:", score);
            UpdateHighScoreText();
        }
    }

    void UpdateHighScoreText() //remembers new highscore.
    {
        highScoreText.text = $"HighScore: {PlayerPrefs.GetInt("High Score:", 0)}";
    }
}
