using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    public Transform cam;
    public GameObject gewis;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = gewis.transform.position;
    }
}
