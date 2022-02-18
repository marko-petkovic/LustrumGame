using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaCameraActivator : MonoBehaviour
{

    public List<EnigmaMovement> movementControl;

    private bool prank = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var col = collision.gameObject;
        if (!prank && col.tag == "Player")
        {
            foreach (var item in movementControl)
            {
                item.enabled = true;
            }
        }
    }
}
