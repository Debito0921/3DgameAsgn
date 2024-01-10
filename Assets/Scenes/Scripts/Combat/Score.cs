using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public GameOver gameOver;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int scoreAdd)
    {
        score += scoreAdd;
        UpdateScore();
    }

    public void DeductScore(int scoreDeduct)
    {
        score -= scoreDeduct;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score : " + score;
    }

    public void DoneGame()
    {
        gameOver.Setup(score);
    }

}
