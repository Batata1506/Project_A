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
    private void Awake()
    {

        body = GetComponent<Rigidbody2D>();
        coreMoveScript = GetComponent<BasicPlayerMovement>();
    }

    private void Update()
    {
       // print(CD);
        if (CD > 10)
            CD = 0;

        if(CD != 0)
        {
            CD += Time.deltaTime;
        }

        if(inDashMode == true)
        {
            body.transform.localScale = new Vector3(initialLocalScale, body.transform.localScale.y, body.transform.localScale.z);
        }
        if (Mathf.Abs(rotationSpeed) >= 500)
        {
            rotationSpeed = 500;
        }
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
            print(rotationSpeed);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown("e") && coreMoveScript.isGrounded() && CD == 0)
        {
            initialLocalScale = body.transform.localScale.x;
            inDashMode = true;
        }
        if (Input.GetKeyUp("e") && inDashMode == true)
        {
            Vector2 test = new Vector2((dashMultiplier * 0.5f) * -Mathf.Sign(body.transform.localScale.x), body.velocity.y);
            CD += 1;
            body.AddForce(test, ForceMode2D.Impulse);
            inDashMode = false;
            print(test);
        }
    }
}
