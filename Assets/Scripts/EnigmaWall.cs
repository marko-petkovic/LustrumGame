using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaWall : MonoBehaviour
{

    public GameObject bloodSplash;

    private PauseMenu pauseMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;

        if (col.tag == "Player")
        {
            var health = col.GetComponent<HealthManager>();
            health.Health -= 50f;

            GameObject effect = Instantiate(bloodSplash, col.transform.position, Quaternion.identity);
            Destroy(effect, .75f);
            Destroy(gameObject);




            if (health.Health <= 0)
            {
                health.DeathHealth();
                pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();
                pauseMenu.DeathMenu();

            }
        }     
    }
}
