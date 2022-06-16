using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform player;
    private BasicPlayerMovement coreScript;
    private Rigidbody2D body;
    private CircleCollider2D circleCollider;
    [SerializeField] public float slopeAngle;
    [SerializeField] private float maxClimableAngle;
    private float xPos;
    public bool goingUphill;
    // Start is called before the first frame update
    private void Awake()
    {
        body = player.GetComponent<Rigidbody2D>();
        circleCollider = player.GetComponent<CircleCollider2D>();
        coreScript = GetComponent<BasicPlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        MaxClimableAngle();
    }
    private void FixedUpdate()
    {
        OnSlope();
        DecreaseSpeedGoingUp();
        Debug.DrawRay(circleCollider.bounds.center +  new Vector3(0, -0.52f, 0), new Vector3(0.8f, 0, 0), Color.red);
       // Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), new Vector3(-0.5f, -0, 0), Color.blue);
        Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), new Vector3(0.8f, 0, 0), Color.green);
        Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), new Vector3(-0.8f, 0, 0), Color.white);
       
    }

   public bool OnSlope()
    {
        RaycastHit2D ray = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), Vector2.right,0.8f, groundLayer);
        RaycastHit2D ray1 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), Vector2.left,0.8f, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.right, 0.52f, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.left, 0.52f, groundLayer);
        RaycastHit2D ray6 = Physics2D.Raycast(circleCollider.bounds.center, Vector2.up, 0.8f, groundLayer);



        if (ray.collider != null)
        {
            slopeAngle = Vector2.Angle(ray.normal, Vector2.up);
            if(body.velocity.y < 0 && (coreScript.IsGrounded() == false || coreScript.enteringSlope))
                body.AddForce(-2 * body.gravityScale * ray.normal);
            if (player.localScale.x > 0)
                goingUphill = false;
            else
                goingUphill = true;
        }

        if (ray1.collider != null)
        {
            slopeAngle = Vector2.Angle(ray1.normal, Vector2.up);
            if (body.velocity.y < 0 && (coreScript.IsGrounded() == false || coreScript.enteringSlope))
                body.AddForce(-2 * body.gravityScale * ray1.normal);
            goingUphill = false;
            if (player.localScale.x > 0)
                goingUphill = true;
            else
                goingUphill = false;
        }

        if (ray2.collider != null)
        {
            slopeAngle = Vector2.Angle(ray1.normal, Vector2.up);
            if ((coreScript.IsGrounded() == false || coreScript.enteringSlope))
                body.AddForce(-Mathf.Abs(body.velocity.x) * ray2.normal);
        }

        if (ray3.collider != null)
        {
            slopeAngle = Vector2.Angle(ray1.normal, Vector2.up);
            if ((coreScript.IsGrounded() == false || coreScript.enteringSlope))
                body.AddForce(-Mathf.Abs(body.velocity.x) * ray3.normal);
        }
        return ray.collider != null || ray1.collider != null || ray2.collider != null || ray3.collider != null || coreScript.enteringSlope == true || ray6.collider != null;
    }

    public bool OnLoop()
    {
        RaycastHit2D ray2 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.right, 0.52f, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.left, 0.52f, groundLayer);
        RaycastHit2D ray4 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.3f, 0, 0), Vector2.up, 0.8f, groundLayer);
        RaycastHit2D ray5 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(-0.3f, 0, 0), Vector2.up, 0.8f, groundLayer);
        RaycastHit2D ray6 = Physics2D.Raycast(circleCollider.bounds.center, Vector2.up, 0.56f, groundLayer);

        if(ray4.collider != null)
        {
            body.AddForce(1.1f *-Mathf.Abs(body.velocity.x) * ray4.normal);
        }
        if(ray5.collider != null)
        {
            body.AddForce(1.1f * -Mathf.Abs(body.velocity.x) * ray5.normal);
        }
        if (ray6.collider != null && ray5.collider == null && ray4.collider == null)
        {
            body.AddForce(1.1f * -Mathf.Abs(body.velocity.x) * ray6.normal);
        }

        return ray2.collider != null || ray3.collider != null || ray4.collider != null || ray5.collider != null || ray6.collider != null;
    }

    private void MaxClimableAngle()
    {
        if(slopeAngle > maxClimableAngle && OnSlope() && Mathf.Abs(body.velocity.y) + Mathf.Abs(body.velocity.x) < 20) 
        {
            if(!OnLoop())
            body.velocity += new Vector2(0, -0.1f);
            print("yes");
        }
    }

    private void DecreaseSpeedGoingUp()
    {
        if(Mathf.Abs(body.velocity.x) + Mathf.Abs(body.velocity.y) < 10 && OnSlope() || OnLoop() && Mathf.Abs(body.velocity.x + body.velocity.y) < 20)
        {
            body.velocity += new Vector2(0, -0.03f);
            print("true");
        }
    }
   
}
