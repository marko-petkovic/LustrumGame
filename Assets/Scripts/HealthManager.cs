using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public float Health = 100f;
    public float maxHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Health;
        if (this.tag == "Player")
        {
          healthBar.SetMaxHealth(maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "Player")
        {
            healthBar.SetHealth(Health);
        }
    }

    public void DeathHealth()
    {
        healthBar.SetHealth(0f);
    }
}
