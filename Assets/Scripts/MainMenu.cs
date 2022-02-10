using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public LevelLoader levelLoader;
    public AudioMixer audioMixer;


    public GameObject title;
    public GameObject howTo;

    public GameObject general;
    public GameObject enemies;
    public GameObject special;


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
}
