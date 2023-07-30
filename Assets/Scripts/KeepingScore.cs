using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepingScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI GoScoretext;
    public TextMeshProUGUI GohighScoretext;

    private int score = 0;
    private int highscore = 0;
    void Start()
    {
        highscore = PlayerPrefs.GetInt("High Score:", highscore);
        highscoreText.text = $"HighScore: {PlayerPrefs.GetInt("High Score:", 0)}";
        GoScoretext.text = scoreText.text;
        GohighScoretext.text = $"HighScore: {PlayerPrefs.GetInt("High Score:", 0)}";
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateScore(1);

        if (score > highscore)
        {
            highscore = score;
            highscoreText.text = "High Score:" + highscore;
        }
    }

    public void UpdateScore(int scoreToAdd) //score is added for every second player is alive.
    {
        score += scoreToAdd;
        scoreText.text = "Score:" + score;
        CheckHighScore();
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
        highscoreText.text = $"High Score: {PlayerPrefs.GetInt("High Score:", 0)}";
    }
}
