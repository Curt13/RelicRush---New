using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board; 

    public Text scoreText;
    public int score;
    public Image scoreBar; 

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        UpdateScoreBar();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString(); 
    }

    public void IncrementScore(int amountToIncrement)
    {
        score += amountToIncrement;
        UpdateScoreBar(); 
    }

    public void UpdateScoreBar()
    {
        if (board != null && scoreBar != null)
        {
           int length = board.scoreGoal.Length;
           scoreBar.fillAmount = (float)score / (float)board.scoreGoal[length - 1];            
        }
    }
}
