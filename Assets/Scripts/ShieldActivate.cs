using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActivate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var col = collision.gameObject;

        if (col.tag == "Player" && !col.GetComponent<Shooting>().extraWalls)
        {
            col.GetComponent<Shooting>().ActivateExtraShield();
            Destroy(gameObject);
        }
    }
}
