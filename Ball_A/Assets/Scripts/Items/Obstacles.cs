using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private EdgeCollider2D edgeCollider; // gets the collider of object
    private CircleCollider2D circleCollider; // gets collider of the ball
   [SerializeField] private GameObject player; // gets the player Object
    private Animator anim; // animates the player

    public void Death() // method that casue the death 
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        anim = GetComponent<Animator>();

        if (IsTouchingBall())
        {
            anim.SetTrigger("death");
            anim.SetBool("respawn", true);
        }
        

    }

    public bool IsTouchingBall() // method to check that the player is touching an obstacle
    {
        circleCollider = player.GetComponent<CircleCollider2D>();
        return circleCollider.IsTouching(edgeCollider);
    }
}
