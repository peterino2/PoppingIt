using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager gStaticInstance; 
    
    private int _currentScore = 0;

    public TMP_Text scoreText;

    public static ScoreManager get()
    {
        return gStaticInstance;
    }

    private void Awake()
    {
        gStaticInstance = this;
    }

    public void IncrementScore()
    {
        _currentScore += 1;
        scoreText.text = "Score: " + _currentScore.ToString();
    }

    public void ResetScore()
    {
        _currentScore = 0;
        scoreText.text = "Score: " + _currentScore.ToString();
    }
    
    public int GetScore()
    {
        return _currentScore;
    }

    public void Start()
    {
        gStaticInstance = this;
        scoreText.text = "Score: " + _currentScore.ToString();
    }
}
