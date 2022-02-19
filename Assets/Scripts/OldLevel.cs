using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldLevel : MonoBehaviour
{

    public Text addScoreText;
    private Transform textLoc;
    private bool addedScore = false;


    private void Start()
    {
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
       

        if (col.tag == "Player" && !addedScore)
        {
            PlayerScore.Score += 500;
            var txt = Instantiate(addScoreText, textLoc);
            txt.text = "Found Original Game!: +500";
            addedScore = true;
        }

    }
}
