using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool GameOver = false;
    public LevelLoader levelLoader;

    public GameObject pauseMenuUI;
    public GameObject winMenuUI;
    public GameObject deathMenuUI;

    public GameObject highScoreMenu;

    public TextMeshProUGUI highText;

    public TMP_InputField inputField;

    public TextMeshProUGUI scoreText;

    private AudioManager audioManager;
    //public HealthManager playerHealth;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
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

        audioManager.Stop("Alarm");

        if (PlayerScore.Score > PlayerPrefs.GetInt("Score10", 0))
        {
            highScoreMenu.SetActive(true);
            var rank = GetRank();
            string txt = string.Format("You are number {0} on the high score list!\n Please enter your name below!", rank.ToString());
            highText.text = txt;
        }

    }

    public void EnterName()
    {
        name = inputField.text;
        UpdateScore(name, PlayerScore.Score);
        highScoreMenu.SetActive(false);
    }


    public void LevelWon()
    {
        levelLoader.LoadNextLevel();
    }

    public void DeathMenu()
    {

        audioManager.Stop("Alarm");
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

    int GetRank()
    {
        var score = PlayerScore.Score;
        if (score > PlayerPrefs.GetInt("Score1", 0))
        {
            return 1;
        }
        else if (score > PlayerPrefs.GetInt("Score2", 0))
        {
            return 2;
        }
        else if (score > PlayerPrefs.GetInt("Score3", 0))
        {
            return 3;
        }
        else if (score > PlayerPrefs.GetInt("Score4", 0))
        {
            return 4;
        }
        else if (score > PlayerPrefs.GetInt("Score5", 0))
        {
            return 5;
        }
        else if (score > PlayerPrefs.GetInt("Score6", 0))
        {
            return 6;
        }
        else if (score > PlayerPrefs.GetInt("Score7", 0))
        {
            return 7;
        }
        else if (score > PlayerPrefs.GetInt("Score8", 0))
        {
            return 8;
        }
        else if (score > PlayerPrefs.GetInt("Score9", 0))
        {
            return 9;
        }
        else 
        {
            return 10;
        }
    }

    void UpdateScore(string name, int score)
    {
        bool scoreUpdated = false;
        int i = 1;

        int prevScore = 0;
        string prevName = "a";

        while (i < 11)
        {
            var currScore = PlayerPrefs.GetInt(string.Format("Score{0}", i), 0);
            var currName = PlayerPrefs.GetString(string.Format("Name{0}", i), "Marko");
            if (score > currScore && !scoreUpdated)
            {
                prevScore = currScore;
                prevName = currName;

                PlayerPrefs.SetInt(string.Format("Score{0}", i), score);
                PlayerPrefs.SetString(string.Format("Name{0}", i), name);

                scoreUpdated = true;
            }
            else if (scoreUpdated)
            {
                PlayerPrefs.SetInt(string.Format("Score{0}", i), prevScore);
                PlayerPrefs.SetString(string.Format("Name{0}", i), prevName);

                prevScore = currScore;
                prevName = currName;
            }
            
            i++;
        }
    }
}
