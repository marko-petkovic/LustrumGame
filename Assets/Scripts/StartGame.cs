using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public LevelLoader levelLoader;
    
    public void StartPlay()
    {
        levelLoader.LoadNextLevel();
    }
}
