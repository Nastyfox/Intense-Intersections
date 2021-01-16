using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Status", menuName = "GameStatus")]
public class GameStatus : ScriptableObject
{
    //Status of the game
    public bool isGameOver;
    public bool isInfinite;
    public bool isGameActive;
    public bool isGameWon;

    //Lives counter
    public float lives;

    //Score counter
    public int score;
    public int difficultyLevel;

    //Update score value based on difficulty level
    public void UpdateScore(int scoreToAdd)
    { 
        score += scoreToAdd * difficultyLevel;
    }    
}
