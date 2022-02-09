using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsConsumable : MonoBehaviour
{

    private GameObject superComp;

    void Awake()
    {
        superComp = GameObject.FindGameObjectWithTag("ComputerSuper");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            superComp.GetComponent<SuperComputer>().EnableGPU();
            Destroy(gameObject);
        }
    }
}
