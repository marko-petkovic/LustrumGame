using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GewisBar : MonoBehaviour
{
    public Transform textLoc;
    public Text addScoreText;

    private bool hasBeenStolen = false;

    // Start is called before the first frame update
    void Start()
    {
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !hasBeenStolen)
        {
            PlayerScore.Score += 150;
            var txt = Instantiate(addScoreText, textLoc);
            txt.text = "Brassed Gewis Bar: +150";
            txt.color = Color.red;
            hasBeenStolen = true;
            Destroy(txt, 10f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
