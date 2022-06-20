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
    private SlopeDetection slopeDetect;
    private float bounceSince;
    public bool isBouncing;
    private float coolDown;
    public bool canBounce;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<BasicPlayerMovement>();
        slopeDetect = GetComponent<SlopeDetection>();
    }

    // Update is called once per frame

    private void Update()
    {
        DisableBounce();
    }
    void FixedUpdate()
    {
        Bounce();
        print(bounceSince);

    }

    private void Bounce()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !moveScript.IsGrounded() && slopeDetect.OnSlope() == false && isBouncing == false && canBounce == true)
        {
            isBouncing = true;
            rb.velocity = new Vector2(rb.velocity.x, -50);
            canBounce = false;
        }
     
        if(isBouncing == true)
        {
            bounceSince += Time.deltaTime;
        }
        if ( isBouncing == true && moveScript.IsGrounded())
        {
       
            rb.velocity = new Vector2(rb.velocity.x*2f, (bounceSince*50f)+25f);
            bounceSince = 0;
            isBouncing=false;
            
        }

    }

    private void DisableBounce()
    {
        
        if (moveScript.IsGrounded())
        {
            coolDown += Time.deltaTime;
        }
        if(coolDown > 0.2f)
        {
            canBounce = true;
            coolDown = 0;
        }
    }

    //DisableBounce enables bounce when the player is on the ground for longer than 0.2f seconds || This is used with the code in the fixed update method of jump in BasicPlayerMovement.
    //This does not work perfectly.



}
