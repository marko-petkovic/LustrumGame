using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperComputer : MonoBehaviour
{

    public List<GameObject> gpus;
    public GameObject switcherino;
    public GameObject switcherino2;

    public bool doubleSwitch = false;

    public Animator superCompAnim;
    public Animator doors;
    public Sprite switchOpen;


    private Vector2 playerTransform;
    private int currGPU = 0;
    public bool allGPUS = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( allGPUS && collision.gameObject.tag == "Player")
        {
            switcherino.GetComponent<SpriteRenderer>().sprite = switchOpen;
            if (doubleSwitch)
                switcherino2.GetComponent<SpriteRenderer>().sprite = switchOpen;
            doors.SetBool("OpenDoor", true);
        }
    }

    public void EnableGPU()
    {
        if (currGPU < gpus.Count)
        {
            gpus[currGPU].SetActive(true);
            currGPU++;
            superCompAnim.SetInteger("Level", currGPU);

            if (currGPU == 3)
                allGPUS = true;
        }
    }
}
