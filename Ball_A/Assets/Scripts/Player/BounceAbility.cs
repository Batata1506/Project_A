using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAbility : MonoBehaviour
{
   private CoreMovement ball;
    private bool bounce = true;
    public float coolDownB = Mathf.Infinity;
    private void Awake()
    {
        ball= GameObject.FindObjectOfType<CoreMovement>();
    }

   

    public void Bounce()
    {
        if (bounce && Input.GetKey(KeyCode.Mouse0) && !ball.isGrounded())
        {
         
            StartCoroutine(BounceDelay());
            bounce = !bounce;

        }

        else
        {
           ball.body.velocity = new Vector2(ball.body.velocity.x, ball.body.velocity.y);
            bounce = true;
        }
        coolDownB = 0;
    }
    IEnumerator BounceDelay()
    {
        yield return new WaitForSeconds(0.43f);
        if( ball.isGrounded())
        {
            ball.body.velocity = new Vector2(ball.body.velocity.x, 25);
            ball.anim.SetTrigger("bounce");
        }
    }
}
