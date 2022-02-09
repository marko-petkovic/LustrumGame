using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperComputer : MonoBehaviour
{

    public List<GameObject> gpus;
    public GameObject switcherino;
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
        if (allGPUS && Vector2.Distance(playerTransform, transform.position+switcherino.transform.position) < 5f)
        {
            switcherino.GetComponent<SpriteRenderer>().sprite = switchOpen;
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
