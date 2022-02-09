using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnigmaMine : MonoBehaviour
{

    AudioManager audioManager;
    Transform textLoc;
    public GameObject suicide;
    public Text addScoreText;
    public CircleCollider2D boomCircle;
    public Animator anim;
    public GameObject explosion;
    public GameObject deathSplash;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
// boomCircle = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            StartCoroutine(Boom());
        }
    }


    public void Explode()
    {
        audioManager.Play("Boom", transform.position);
        boomCircle.enabled = true;
        List<Collider2D> colls = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        boomCircle.OverlapCollider(filter, colls);

        var suicideEffect = Instantiate(suicide, transform.position, transform.rotation);
        foreach (var col in colls)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.tag == "Destrucitble")
            {
                audioManager.Play("Wall", transform.position);
                PlayerScore.Score -= 15;
                var txt = Instantiate(addScoreText, textLoc);
                txt.text = "Computer Destroyed: -15";
                txt.color = Color.cyan;
                Destroy(col.gameObject);
                Destroy(txt, 10f);

                GameObject effect = Instantiate(explosion, col.transform.position, transform.rotation);
                Destroy(effect, .75f);

            }
            else if (col.gameObject.tag == "Player")
            {
                var health = col.gameObject.GetComponent<HealthManager>();
                health.DeathHealth();
                var rot = new Quaternion();
                rot.eulerAngles = new Vector3(90, 90, 90);
                GameObject effect = Instantiate(deathSplash, col.transform.position, rot);
                Destroy(col.gameObject);
                Destroy(effect, .75f);
                PauseMenu pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();

                pauseMenu.DeathMenu();
            }
        }
        Destroy(suicideEffect, 5f);
        Destroy(gameObject);
    }

    public IEnumerator Boom()
    {
        audioManager.Play("Alarm");
        anim.SetBool("Boom", true);
        yield return new WaitForSeconds(.5f);
        boomCircle.enabled = true;
        List<Collider2D> colls = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        boomCircle.OverlapCollider(filter, colls);

        var suicideEffect = Instantiate(suicide, transform.position, transform.rotation);
        audioManager.Stop("Alarm");
        audioManager.Play("Boom", transform.position);

        foreach (var col in colls)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.tag == "Destrucitble")
            {
                audioManager.Play("Wall", transform.position);
                PlayerScore.Score -= 15;
                var txt = Instantiate(addScoreText, textLoc);
                txt.text = "Computer Destroyed: -15";
                txt.color = Color.cyan;
                Destroy(col.gameObject);
                Destroy(txt, 10f);

                GameObject effect = Instantiate(explosion, col.transform.position, transform.rotation);
                Destroy(effect, .75f);

            }
            else if (col.gameObject.tag == "Player")
            {
                var health = col.gameObject.GetComponent<HealthManager>();
                health.DeathHealth();
                var rot = new Quaternion();
                rot.eulerAngles = new Vector3(90, 90, 90);
                GameObject effect = Instantiate(deathSplash, col.transform.position, rot);
                Destroy(col.gameObject);
                Destroy(effect, .75f);
                PauseMenu pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();

                pauseMenu.DeathMenu();
            }
        }
        Destroy(suicideEffect, 5f);
        Destroy(gameObject);

    }
}
