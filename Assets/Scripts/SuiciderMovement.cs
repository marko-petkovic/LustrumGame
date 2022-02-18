using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class SuiciderMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float forgetTime = 1.5f; //how long the gewisser can see pattern through walls 
    public float distanceTres = 5f;

    public GameObject explosion;
    public GameObject deathSplash;
    public GameObject suicide;

    public Animator animator;

    public float nextWaypointDistance = 3f;

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
    private bool returningToPatrol = false;


    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;


    [SerializeField]
    private Transform[] waypoints;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    AudioManager audioManager;
    Transform textLoc;

    public Text addScoreText;

    private CircleCollider2D boomCircle;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
        boomCircle = GetComponent<CircleCollider2D>();
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
                
                if (hit.collider.tag == "Player")
                {
                    if (!chasing)
                    {
                        animator.SetBool("chasing", true);
                        audioManager.Play("Alarm");
                    }
                    returningToPatrol = false;
                    chasing = true;
                    
                    ChaseCharacter();
                    lastSeen = DateTime.Now;
                   
                }
                else if ((DateTime.Now - lastSeen).TotalSeconds < forgetTime)
                {
                    chasing = true;
                    returningToPatrol = false;
                    ChaseCharacter();
                }
                else
                {
                    if (chasing)
                    {
                        chasing = false;
                        animator.SetBool("chasing", false);
                        audioManager.Stop("Alarm");
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

    public void Boom()
    {
        boomCircle.enabled = true;
        List<Collider2D> colls = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        boomCircle.OverlapCollider(filter, colls);

        var suicideEffect = Instantiate(suicide, transform.position, transform.rotation);
        audioManager.Stop("Alarm");
        audioManager.Play("Boom", transform.position);


        foreach (var col in colls)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.tag == "Destrucitble")
            {
                audioManager.Play("Wall", transform.position);
                PlayerScore.Score -= 15;
                var txt = Instantiate(addScoreText, textLoc);
                txt.text = "Computer Destroyed: -15";
                txt.color = Color.cyan;
                Destroy(col.gameObject);
                Destroy(txt, 10f);

                GameObject effect = Instantiate(explosion, col.transform.position, transform.rotation);
                Destroy(effect, .75f);
                
            }
            else if (col.gameObject.tag == "Player")
            {
                var health = col.gameObject.GetComponent<HealthManager>();
                health.DeathHealth();
                var rot = new Quaternion();
                rot.eulerAngles = new Vector3(90, 90, 90);
                GameObject effect = Instantiate(deathSplash, col.transform.position, rot);
                Destroy(col.gameObject);
                Destroy(effect, .75f);
                PauseMenu pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();

                pauseMenu.DeathMenu();
            }
        }

        Destroy(suicideEffect, 5f);
        Destroy(gameObject);
    }

    void ChaseCharacter()
    {
        // if the player is close, go Boom!
        float distance = GetDistance(transform, player); // Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
        if (distance <= distanceTres)
        {
            Boom();
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
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
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
    
