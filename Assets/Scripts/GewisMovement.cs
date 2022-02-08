using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GewisMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float forgetTime = 1.5f; //how long the gewisser can see pattern through walls 
    public float reloadTime = 2f;
    public float burstTime = 0.1f;
    public float bulletForce = 10f;
    public int burstLength = 3;


    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject playerObject;

    private Transform player;
    public Transform eyes;

    private Rigidbody2D rb;
    private Vector2 movement;
    private DateTime lastSeen = DateTime.Now.AddSeconds(-10d);
    private DateTime lastShot = DateTime.Now.AddSeconds(-10d);
    private DateTime emptyMag = DateTime.Now.AddSeconds(-10d);
    private int bulletsInMag;
    private bool patrolForward = true;
    private bool chasing = false;
    



    [SerializeField]
    private Transform[] waypoints;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        bulletsInMag = burstLength;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject == null)
        {
            Patrol();
        }
        else
        {
            player = playerObject.transform;
            Vector3 targetDir = player.position - transform.position;
            float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
            RaycastHit2D hit;

            Vector3 direction = player.position - eyes.position;

            //Debug.DrawRay(eyes.position, direction);

            hit = (Physics2D.Raycast(eyes.position, direction));
            if (hit.collider != null)
            {
                //Debug.Log(hit.transform.name);
                //Debug.Log(hit.collider.name);
                if (hit.collider.tag == "Player")
                {
                    chasing = true;
                    //Debug.Log("Player spotted!");
                    ChaseCharacter();
                    lastSeen = DateTime.Now;
                    if (!PauseMenu.GameIsPaused)
                    {
                        if (bulletsInMag > 0)
                        {
                            if ((DateTime.Now - lastShot).TotalSeconds > burstTime)
                            {
                                Shoot();
                                bulletsInMag -= 1;
                                lastShot = DateTime.Now;
                                if (bulletsInMag == 0)
                                {
                                    emptyMag = DateTime.Now;
                                }
                            }

                        }
                        else if ((DateTime.Now - emptyMag).TotalSeconds > reloadTime)
                        {
                            bulletsInMag = burstLength;
                            Shoot();
                            bulletsInMag -= 1;
                            lastShot = DateTime.Now;
                            if (bulletsInMag == 0)
                            {
                                emptyMag = DateTime.Now;
                            }
                        }
                    }
                }
                else if ((DateTime.Now - lastSeen).TotalSeconds < forgetTime)
                {
                    chasing = true;
                    ChaseCharacter();
                }
                else
                {
                    if (chasing)
                    {
                        chasing = false;
                        int bestWayPoint = 0;
                        double minDist = Mathf.Infinity;
                        for (int i = 0; i < waypoints.Length; i++)
                        {
                            double dist = Math.Pow(transform.position.x - waypoints[i].position.x, 2d) + Math.Pow(transform.position.y - waypoints[i].position.y, 2d);
                            if (dist < minDist)
                            {
                                bestWayPoint = i;
                                minDist = dist;
                            }
                        }
                        waypointIndex = bestWayPoint;
                    }
                    Patrol();
                }
            }
            else
            {
                //Debug.Log("no hit!");
            }
        }

    }

    private void FixedUpdate()
    {


    }

    void Shoot()
    {
        //Quaternion randomRotation = firePoint.rotation;
        //var rand = new System.Random();
        //float offset = 5f*((float)rand.NextDouble() - 0.5f);
        //Debug.Log(offset);
        //randomRotation.z += offset;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void ChaseCharacter()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        transform.position = Vector2.MoveTowards(transform.position,
                    player.position,
                     moveSpeed * Time.deltaTime);
        //rb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    void Patrol()
    {
        if (waypointIndex <= waypoints.Length - 1 && patrolForward)
        {

            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            Vector3 direction = waypoints[waypointIndex].transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);
            
            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
        else if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = waypoints.Length - 2;
            patrolForward = false;
        }
        else if (waypointIndex == 0 && !patrolForward)
        {
            patrolForward = true;
        }
        else
        {
            Vector3 direction = waypoints[waypointIndex].transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex -= 1;
            }
        }
        
        
    }

}
