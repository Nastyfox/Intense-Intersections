using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Enumeration for game modes
    public enum gameModes { EASY = 1, NORMAL = 2, HARD = 3, INFINITE = 4 }

    //Scriptable object for game status
    public GameStatus gameStatus;

    //Variables to manage main menu and start of the game
    private GameObject titleScreen;
    private GameObject gameOverScreen;
    private GameObject gameWonScreen;
    private SpawnManager spawnManager;

    //Variables for scoring and lives in infinite mode
    const string scoreKey = "High Score";
    private int lastHighScore;
    public TextMeshProUGUI highScoreText;
    private GameObject livesObject;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    //Variables for timer in easy, normal and hard mode
    private float timer;
    private float timerDuration = 20;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        //Set Game status elements
        gameStatus.isGameActive = false;
        gameStatus.isGameOver = false;
        gameStatus.isGameWon = false;
        gameStatus.isInfinite = false;

        //Get elements to manage start of the game
        titleScreen = GameObject.Find("TitleScreen");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        spawnManager.gameObject.SetActive(false);
        timer = timerDuration;

        //Get elements to manage lives
        livesObject = GameObject.Find("Canvas").transform.Find("Lives").gameObject;

        //Get elements to manage end of the game
        gameOverScreen = GameObject.Find("Canvas").transform.Find("GameOverScreen").gameObject;
        gameWonScreen = GameObject.Find("Canvas").transform.Find("GameWonScreen").gameObject;
    }

    private void Update()
    {
        //Update timer in every mode
        if(gameStatus.isGameActive)
        {
            UpdateTimer();
            DisplayScore();

            //Update lives if infinite mode
            if (gameStatus.isInfinite)
            {
                DisplayLives();
            }
        }
        
        //If game is over launch game over screen
        if(gameStatus.isGameOver)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        //Game is over and show the game over screen to restart
        gameOverScreen.SetActive(true);
        gameStatus.isGameActive = false;

        //Save high score in infinite mode
        if(gameStatus.isInfinite)
        {
            SaveHighScore();
        }
    }

    public void StartGame(int difficulty)
    {
        //Set scoring level to the difficult value
        gameStatus.difficultyLevel = difficulty;

        //Check if infinite mode chosen
        if(difficulty == (int)gameModes.INFINITE)
        {
            //Enable scoring and lives
            gameStatus.isInfinite = true;
            scoreText.enabled = true;
            livesObject.SetActive(true);

            //Set difficulty to 1 for infinite mode
            gameStatus.difficultyLevel = 1;

            //Set lives
            gameStatus.lives = 3;
        }
        else
        {
            //Enable timer
            gameStatus.isInfinite = false;
            timerText.enabled = true;
            timer *= difficulty;
        }

        //Deactivate title screen, activate spawning based on difficulty level and change timer before start the game
        titleScreen.SetActive(false);
        spawnManager.gameObject.SetActive(true);
        spawnManager.SetDifficulty(difficulty);
        gameStatus.score = 0;
        gameStatus.isGameActive = true;
    }

    public void Restart()
    {
        //Reload entire scene to make the game restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void Quit()
    {
        //Quit the game
        Application.Quit();
    }

    private void UpdateTimer()
    {
        //Change timer at each frame and use round to only display seconds
        timer -= Time.deltaTime;
        timerText.text = "Time : " + Mathf.Round(timer);
        
        if(timer <= 0)
        {
            //Timer is over so game is won in easy, normal and hard mode
            if (!gameStatus.isInfinite)
            {
                gameStatus.isGameWon = true;
                gameStatus.isGameActive = false;
                gameWonScreen.SetActive(true);
                timerText.enabled = false;
            }
            //In infinite mode, increase difficulty
            else
            {
                spawnManager.IncreaseDifficulty(2);
                gameStatus.difficultyLevel++;
                timer = timerDuration;
            }
        }
    }

    private void SaveHighScore()
    {
        //Load high score
        LoadHighScore();

        //Check if new score is greater than previous one and if it is so, save the new one
        if(gameStatus.score > lastHighScore)
        {
            lastHighScore = gameStatus.score;
            PlayerPrefs.SetInt(scoreKey, gameStatus.score);
            PlayerPrefs.Save();
        }

        //Display high score
        highScoreText.enabled = true;
        highScoreText.text = "High Score : " + lastHighScore;
    }

    private void LoadHighScore()
    {
        //Get last high score saved
        lastHighScore = PlayerPrefs.GetInt(scoreKey, 0);
    }

    public void DisplayScore()
    {
        scoreText.text = "Score : " + gameStatus.score;
    }

    public void DisplayLives()
    {
        livesText.text = "x " + gameStatus.lives;
    }
}
