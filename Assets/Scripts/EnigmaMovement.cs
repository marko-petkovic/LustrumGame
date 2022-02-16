using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnigmaMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float forgetTime = 1.5f; //how long the gewisser can see pattern through walls 
    public float reloadTime = 2f;
    public float burstTime = 0.1f;
    public float bulletForce = 10f;
    public int burstLength = 3;
    public float distanceTres = 5f;

    public bool shotgun = false;
    public bool doublegun = false;
    public bool boss = false;

    public float nextWaypointDistance = 3f; 

    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public GameObject bulletPrefab;
    public GameObject playerObject;

    public float bulletDamage = 50f;

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
    private bool returningToPatrol = false;

    private Animator anim;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;


    [SerializeField]
    private Transform[] waypoints;

    // Index of current waypoint from which Enemy walks
    // to the next one
    public int waypointIndex = 0;


    private void Awake()
    {
        if (boss)
            anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        bulletsInMag = burstLength;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (true || chasing || returningToPatrol)
        {
            if (seeker.IsDone() && !returningToPatrol && playerObject != null)
                seeker.StartPath((Vector2)transform.position, (Vector2)playerObject.transform.position, OnPathComplete);
            else if (seeker.IsDone() && returningToPatrol)
                seeker.StartPath(transform.position, waypoints[waypointIndex].position, OnPathComplete);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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
                if (hit.collider.tag == "Player" || hit.collider.tag == "PlayerWall")
                {

                    if (!chasing && boss)
                    {
                        anim.SetBool("chasing", true);
                    }

                    returningToPatrol = false;
                    chasing = true;
                    //Debug.Log("Player spotted!");
                    ChaseCharacter(true);
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
                    returningToPatrol = false;
                    ChaseCharacter(false);
                }
                else if ((Vector2.Distance(transform.position, player.transform.position)) < 30f)
                {
                    chasing = true;
                    returningToPatrol = false;
                    ChaseCharacter(false);
                    lastSeen = DateTime.Now;
                }

                else
                {
                    if (chasing)
                    {
                        if (boss)
                            anim.SetBool("chasing", false);
                        chasing = false;
                        returningToPatrol = true;
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
                    else if (returningToPatrol)
                    {
                        if (GetDistance(transform, waypoints[waypointIndex]) < 2f)
                        {
                            returningToPatrol = false;
                            Patrol();
                        }
                        else
                        {
                            MoveBack();
                        }
                    }
                    else
                        Patrol();
                }
            }
            else
            {
                //Debug.Log("no hit!");
            }
        }

    }


    void Shoot()
    {
        //Quaternion randomRotation = firePoint.rotation;
        //var rand = new System.Random();
        //float offset = 5f*((float)rand.NextDouble() - 0.5f);
        //Debug.Log(offset);
        //randomRotation.z += offset;
        if (shotgun)
        {
            List<float> bulletRotations = new List<float> {-.15f,-.125f,-.1f, .075f,-0.05f, -0.025f, 0f, 0.025f, .05f, 0.075f, .1f, .125f,.15f};
            for (int i = 0; i < bulletRotations.Count; i++)
            {
                var bulRot = firePoint.rotation;
                bulRot.z += bulletRotations[i];
                var bullet = Instantiate(bulletPrefab, firePoint.position, bulRot);
                bullet.GetComponent<Bullet>().damage = bulletDamage;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }
        }
        else{
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<Bullet>().damage = bulletDamage;
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            if (doublegun || boss)
            {
                GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
                bullet2.GetComponent<Bullet>().damage = bulletDamage;
                Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                rb2.AddForce(firePoint2.up * bulletForce, ForceMode2D.Impulse);
            }
            if (boss)
            {
                GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.position, firePoint3.rotation);
                bullet3.GetComponent<Bullet>().damage = bulletDamage*0.5f;
                Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
                rb3.AddForce(firePoint2.up * bulletForce, ForceMode2D.Impulse);
            }
        }
        //GameObject bullet = 
        
    }

    void ChaseCharacter(bool playerVisible)
    {
        // if the enemy is too close, do not chase the player
        float distance = GetDistance(transform, player); // Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
        if (playerVisible &&  distance <= distanceTres)
        {
            Debug.Log("too close!");
            return;
        }
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2) player.position - (Vector2) transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // enemy needs to point to player to be able to shoot them
        rb.rotation = angle;
        //var pathDir = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position,
                    path.vectorPath[currentWaypoint],
                     moveSpeed * Time.deltaTime);

        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        //rb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    void MoveBack()
    {
        if (path == null)
        {
            return;
        }
        Vector2 direction = ((Vector2)waypoints[waypointIndex].position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        //var pathDir = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position,
                    path.vectorPath[currentWaypoint],
                     moveSpeed * Time.deltaTime);
        var distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    float GetDistance(Transform tr1, Transform tr2)
    {
        return Vector2.Distance((Vector2)tr1.position, (Vector2)tr2.position);
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
            if (Vector2.Distance(transform.position, waypoints[waypointIndex].transform.position) < .3f)
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
            if (Vector2.Distance(transform.position, waypoints[waypointIndex].transform.position) < .3f)
            {
                waypointIndex -= 1;
            }
        }


    }

}
