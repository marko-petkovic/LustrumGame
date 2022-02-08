using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool GameOver = false;
    public LevelLoader levelLoader;

    public GameObject pauseMenuUI;
    public GameObject winMenuUI;
    public GameObject deathMenuUI;
    public PlayerVictory grolschKanon;
    public Text scoreText;
    //public HealthManager playerHealth;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&!GameOver)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void FixedUpdate()
    {
        scoreText.text = PlayerScore.Score.ToString();
    }

    public void WinMenu()
    {
        GameOver = true;
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LevelWon()
    {
        levelLoader.LoadNextLevel();
    }

    public void DeathMenu()
    {
        GameOver = true;
        deathMenuUI.SetActive(true);
    }

    public void Restart()
    {
        
        Time.timeScale = 1f;
        GameOver = false;
        GameIsPaused = false;
        PlayerScore.SetDefaultValues();
        levelLoader.StartLevel(1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        GameOver = false;
        levelLoader.StartLevel(0);
        //SceneManager.LoadScene(0);
        
    }
}
