using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variable to manage game over state
    public bool isGameOver;

    //Variables to manage main menu and start of the game
    private GameObject titleScreen;
    private SpawnManager spawnManager;
    private bool gameStarted;

    private void Start()
    {
        //Get elements to manage start of the game
        titleScreen = GameObject.Find("TitleScreen");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        spawnManager.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        //Game is over
        isGameOver = true;
    }

    public void StartGame(int difficulty)
    { 
        //Deactivate title screen, activate spawning based on difficulty level and start the game
        titleScreen.SetActive(false);
        spawnManager.gameObject.SetActive(true);
        spawnManager.SetDifficulty(difficulty);
        gameStarted = true;
    }
}
