using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    //left and right movement
    public float moveDirection;
    bool grounded;
    [SerializeField] float speed = 1;

    //raycasting
    [SerializeField] float rayDistance = 1f;
    [SerializeField] LayerMask layerMask;

    //jumping
    float spaceTime;
    float charge;
    [SerializeField] float maxCharge = 1f;
    [SerializeField] float colorCharge = 1.6f;
    [SerializeField] float xJumpMultiplier = 1f;
    [SerializeField] float yJumpMultiplier = 1f;
    bool groundedSpacePressed = false;

    //bool maxChargeReached = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.Raycast(
            gameObject.transform.position,
            Vector2.down,
            rayDistance,
            layerMask
        );

        if (raycastHit2D.collider != null && rb.velocity.y <= 0)
        {
            grounded = true;
            rb.velocity = new Vector2(0, 0);
            //Debug.Log("Ray hit " + raycastHit2D.collider.name);
        }
        else
        {
            grounded = false;
        }

        moveDirection = Input.GetAxis("Horizontal");

        

        if (grounded)
        {
            //movement
            if (!Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
            }
            //jump charge color change
            else if (groundedSpacePressed)
            {
                GetComponent<SpriteRenderer>().color = new Color(
                    GetComponent<SpriteRenderer>().color.r,
                    GetComponent<SpriteRenderer>().color.g - (colorCharge * Time.deltaTime),
                    GetComponent<SpriteRenderer>().color.b - (colorCharge * Time.deltaTime),
                    GetComponent<SpriteRenderer>().color.a
                    );
            }

            //jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spaceTime = Time.time;
                rb.velocity = new Vector2(0, rb.velocity.y);
                groundedSpacePressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && groundedSpacePressed)
            {
                charge = Time.time - spaceTime;
                if (charge > maxCharge)
                {
                    charge = maxCharge;
                }

                if (moveDirection > 0)
                {
                    moveDirection = 1;
                }
                else if (moveDirection < 0)
                {
                    moveDirection = -1;
                }

                rb.velocity = new Vector2(moveDirection * xJumpMultiplier, charge * yJumpMultiplier);
                GetComponent<SpriteRenderer>().color = Color.white;

                groundedSpacePressed = false;
            }
        }

        

    }
}
