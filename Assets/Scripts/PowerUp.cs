using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var col = collision.gameObject;

        if (col.tag == "Player" && !col.GetComponent<Shooting>().extraGuns)
        {
            col.GetComponent<Shooting>().ActivateExtraGuns();
            Destroy(gameObject);
        }
    }
}
