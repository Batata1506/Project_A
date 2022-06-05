using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAbility : MonoBehaviour
{
   private CoreMovement ball;
      private bool bounce = true;

    private void Awake()
    {
        ball= GameObject.FindObjectOfType<CoreMovement>();
    }

   

    public void Bounce()
    {
        if (bounce && Input.GetKey(KeyCode.Mouse0) && !ball.isGrounded())
        {
            ball.body.AddForce(new Vector2(ball.body.velocity.x, 0), ForceMode2D.Force);
            //body.velocity = new Vector2(body.velocity.x, body.velocity.y *0);
            StartCoroutine(BounceDelay());
            bounce = false;

        }

        else
        {
            ball.body.velocity = new Vector2(ball.body.velocity.x, ball.body.velocity.y);
            bounce = true;
        }

    }
    IEnumerator BounceDelay()
    {
        yield return new WaitForSeconds(0.43f);
        if(ball.isGrounded())
        {
            ball.body.AddForce(new Vector2(ball.body.velocity.x, ball.jumpHeight * 10), ForceMode2D.Force);
            //body.velocity = new Vector2(body.velocity.x, jumpHeight + 10);
            ball.anim.SetTrigger("jump");
        }
    }
}
