using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private BasicPlayerMovement coreMoveScript;
    private Rigidbody2D body;
    [Header("Dash")]
    [SerializeField] private float dashMultiplier;
    private float initialLocalScale;
    private bool inDashMode;
    private float rotationSpeed = 0;
    private float CD;
    private SlopeDetection slopeScript;
    private void Awake()
    {

        body = GetComponent<Rigidbody2D>();
        coreMoveScript = GetComponent<BasicPlayerMovement>();
        slopeScript = GetComponent<SlopeDetection>();
    }

    private void Update()
    {
       // print(CD);
        if (CD > 5) //COOLDOWN
            CD = 0;

        if(CD != 0)
        {
            CD += Time.deltaTime;
        }

        if(inDashMode == true)
        {
            body.transform.localScale = new Vector3(initialLocalScale, body.transform.localScale.y, body.transform.localScale.z);
        }
        //if (Mathf.Abs(rotationSpeed) >= 500)
        //{
           // rotationSpeed = 500;
        //}
        Dash();
    }
    private void FixedUpdate()
    {
        if (inDashMode)
        {

            dashMultiplier += 1;
            body.velocity = new Vector2(0, body.velocity.y);
            rotationSpeed += 0.5f * dashMultiplier * Mathf.Sign(body.transform.localScale.x);
            body.rotation = rotationSpeed;

            if(dashMultiplier > 200)
            {
                dashMultiplier = 200;
            }
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown("e") && coreMoveScript.IsGrounded() && CD == 0)
        {
            initialLocalScale = body.transform.localScale.x;
            inDashMode = true;
        }
        if (Input.GetKeyUp("e") && inDashMode == true)
        {
            Vector2 test = new Vector2((dashMultiplier * 0.4f) * -Mathf.Sign(body.transform.localScale.x), body.velocity.y);
            CD += 1;
            if (slopeScript.OnSlope() == true && slopeScript.goingUphill && slopeScript.slopeAngle != 90)
            {
                body.AddForce(new Vector2((dashMultiplier * 0.4f) * -Mathf.Sign(body.transform.localScale.x) * Mathf.Cos(slopeScript.slopeAngle * Mathf.Deg2Rad), body.velocity.y * Mathf.Sin(slopeScript.slopeAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
            }
            else if(slopeScript.OnSlope() == true && slopeScript.goingUphill == false && slopeScript.slopeAngle != 90)
            {
                body.AddForce(new Vector2((dashMultiplier * 0.4f) * -Mathf.Sign(body.transform.localScale.x) * Mathf.Cos(slopeScript.slopeAngle * Mathf.Deg2Rad), body.velocity.y * -Mathf.Sin(slopeScript.slopeAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
            }
            else if(slopeScript.OnSlope() == true && slopeScript.slopeAngle == 90)
            {
                body.AddForce(new Vector2(body.velocity.x, dashMultiplier * 0.4f * -Mathf.Sign(body.transform.localScale.x)));
            }

            else
            {
                body.AddForce(test, ForceMode2D.Impulse);
            }
            inDashMode = false;
            print(test);
        }
    }
}
