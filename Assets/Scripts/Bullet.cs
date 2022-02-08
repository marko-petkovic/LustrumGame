using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public GameObject deathSplash;
    public GameObject bloodSplash;
    public GameObject explosion;
    public float damage = 50;

    private Transform textLoc;
    public Text addScoreText;


    private AudioManager audioManager;

    private PauseMenu pauseMenu;


    private HealthManager health;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        var instPos = col.transform.position;


        if (col.tag == "Gewis" || col.tag == "Player" || col.tag == "Boomer")
        {
            audioManager.Play("Body", transform.position);
            health = col.GetComponent<HealthManager>();
            health.Health -= damage;

            if (health.Health <= 0)
            {
                if (col.tag == "Player")
                {
                    health.DeathHealth();
                    pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();
                    pauseMenu.DeathMenu();
                }
                if (col.tag == "Gewis")
                {
                    PlayerScore.Score += 50;
                    var txt = Instantiate(addScoreText, textLoc);
                    txt.text = "Enemy killed: +50";
                    Destroy(txt, 10f);
                }
                if (col.tag == "Boomer")
                {
                    PlayerScore.Score += 100;
                    var txt = Instantiate(addScoreText, textLoc);
                    txt.text = "Suicider killed: +75";
                    col.GetComponent<SuiciderMovement>().Boom();
                }
                else
                {
                    GameObject effect = Instantiate(deathSplash, col.transform.position, Quaternion.identity);

                    Destroy(effect, .75f);
                    Destroy(col);
                }
                Destroy(gameObject);

            }
            else
            {
                GameObject effect = Instantiate(bloodSplash, col.transform.position, Quaternion.identity);
                Destroy(effect, .75f);
                Destroy(gameObject);
            }
        }
        else if (col.tag == "Wall")
        {
            audioManager.Play(name: "Wall", soundPos: transform.position);
            GameObject effect = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(effect, .75f);
            Destroy(gameObject);
        }
        else if (col.tag == "Destrucitble")
        {
            audioManager.Play("Wall", transform.position);
            PlayerScore.Score -= 15;
            var txt = Instantiate(addScoreText, textLoc);
            txt.text = "Computer Destroyed: -15";
            txt.color = Color.cyan;
            Destroy(col);
            Destroy(txt, 10f);

            GameObject effect = Instantiate(explosion, instPos, transform.rotation);
            Destroy(effect, .75f);
            Destroy(gameObject);
        }

    }
}
