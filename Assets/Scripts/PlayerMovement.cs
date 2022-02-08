using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;


    Vector2 movement;


    void Start()
    {
        Physics2D.IgnoreLayerCollision(9,10,true);
    }


    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.y) > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angle = angle - 90f;
        if (!PauseMenu.GameIsPaused)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+movement*moveSpeed*Time.fixedDeltaTime);
    }
}
