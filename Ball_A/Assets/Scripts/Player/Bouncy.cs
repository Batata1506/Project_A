using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  What is needed? 
 *  When in the air a button is pressed that adds a force to the ball and once it hits the ground. (Few conditons requried here to check if the ball is in the air or ground, condtion to check if the button is pressed)
 *  we need to equal the velocity of the drop and x1.5 it to creat the bounce effect.
 */

public class Bouncy : MonoBehaviour
{
   private Rigidbody2D rb;
    private BasicPlayerMovement moveScript;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<BasicPlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Bounce();
    }

    private void Bounce()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !moveScript.IsGrounded())
        {

            rb.AddForce(new Vector2(0, -rb.velocity.y));

            /*
            if (IsAboutToTouchGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y) * 1.5f);
            }
            */
        }
    }

   

}
