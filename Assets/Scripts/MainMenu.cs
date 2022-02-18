using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public LevelLoader levelLoader;
    public AudioMixer audioMixer;


    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highNameText;

    public GameObject title;
    public GameObject howTo;
    public GameObject highScores;

    public GameObject general;
    public GameObject enemies;
    public GameObject special;

    private void Start()
    {
        GetHighScores();
    }
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //levelLoader.StartLevel(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerScore.SetDefaultValues();
        levelLoader.StartLevel(1);
    }

    public void Special()
    {
        special.SetActive(true);
        enemies.SetActive(false);
        general.SetActive(false);
    }

    public void Enemies()
    {
        special.SetActive(false);
        enemies.SetActive(true);
        general.SetActive(false);
    }

    public void General()
    {
        special.SetActive(false);
        enemies.SetActive(false);
        general.SetActive(true);
    }

    public void GameOptions()
    {
        mainMenu.SetActive(false);
        title.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void HowToPlay()
    {
        title.SetActive(false);
        mainMenu.SetActive(false);
        howTo.SetActive(true);
    }

    public void ReturnToMenu()
    {
        mainMenu.SetActive(true);
        title.SetActive(true);
        optionsMenu.SetActive(false);
        howTo.SetActive(false);
        highScores.SetActive(false);
    }


    public void HighScore()
    {
        mainMenu.SetActive(false);
        highScores.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    public void GetHighScores()
    {
        string names = "";
        names += PlayerPrefs.GetString("Name1", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name2", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name3", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name4", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name5", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name6", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name7", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name8", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name9", "Marko") + "\n";
        names += PlayerPrefs.GetString("Name10", "Marko");

        highNameText.text = names;

        string scores = "";
        scores += PlayerPrefs.GetInt("Score1", 10).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score2", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score3", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score4", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score5", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score6", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score7", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score8", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score9", 0).ToString() + "\n";
        scores += PlayerPrefs.GetInt("Score10", 0).ToString();

        highScoreText.text = scores;
    }
}
