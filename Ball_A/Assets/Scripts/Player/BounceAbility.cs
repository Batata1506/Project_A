using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAbility : MonoBehaviour
{
   private CoreMovement ball;
    private bool bounce = true;
    public float coolDownB = Mathf.Infinity;
    private float canBounce = 0;
    private float bounceCD;
    private void Awake()
    {
        ball= GameObject.FindObjectOfType<CoreMovement>();
    }

    private void FixedUpdate()
    {
        print(canBounce);
        if (ball.isGrounded() && bounce)
        {
            ball.body.velocity = new Vector2(ball.body.velocity.x, 25);
            bounce = false;
        }
    }

    public void Bounce()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !ball.isGrounded() && canBounce > 0)
        {
            bounce = true;
            ball.body.AddForce(new Vector2(0, -100));
        }
    }
}
