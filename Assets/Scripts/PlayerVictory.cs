using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictory : MonoBehaviour
{
    

    public PauseMenu pauseMenu;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            pauseMenu.WinMenu();
            Debug.Log("You Won!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
