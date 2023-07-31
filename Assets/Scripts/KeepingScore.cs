using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepingScore : MonoBehaviour
{
    GameObject gameManager;
    Pause pause;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI GoScoretext;
    public TextMeshProUGUI GohighScoretext;

    public int Score { get; private set; } = 0;
    private int highscore = 0;
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        pause = gameManager.GetComponent<Pause>();

        highscore = PlayerPrefs.GetInt("High Score:", highscore);
        highscoreText.text = $"HighScore: {PlayerPrefs.GetInt("High Score:", 0)}";
        GoScoretext.text = scoreText.text;
        GohighScoretext.text = $"HighScore: {PlayerPrefs.GetInt("High Score:", 0)}";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pause.IsGameRunning && !pause.IsGameOver)
            UpdateScore(1);

        if (Score > highscore)
        {
            highscore = Score;
            highscoreText.text = "High Score:" + highscore;
        }
    }

    public void UpdateScore(int scoreToAdd) //Score is added for every second player is alive.
    {
        Score += scoreToAdd;
        scoreText.text = "Score:" + Score;
        CheckHighScore();
    }

    void CheckHighScore() //if player beats highscore, remembers new highscore.
    { 
        if (Score > PlayerPrefs.GetInt("High Score:", 0)) 
        {
            PlayerPrefs.SetInt("High Score:", Score);
            UpdateHighScoreText();
        }
    }

    void UpdateHighScoreText() //remembers new highscore.
    {
        highscoreText.text = $"High Score: {PlayerPrefs.GetInt("High Score:", 0)}";
    }
}
