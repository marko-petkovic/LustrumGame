using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVictory : MonoBehaviour
{
    public PauseMenu pauseMenu;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            pauseMenu.LevelWon();
            Debug.Log("Next Level!");
        }
    }
}
