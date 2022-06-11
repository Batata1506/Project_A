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
    [SerializeField] private float slopeAngle;
    // Start is called before the first frame update
    private void Awake()
    {
        body = player.GetComponent<Rigidbody2D>();
        circleCollider = player.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void FixedUpdate()
    {
        OnSlope();
        Debug.DrawRay(circleCollider.bounds.center +  new Vector3(0, -0.52f, 0), new Vector3(0.5f, 0, 0), Color.red);
       // Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), new Vector3(-0.5f, -0, 0), Color.blue);
       // Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), new Vector3(0.5f, 0, 0), Color.green);
       // Debug.DrawRay(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), new Vector3(0.5f, 0, 0), Color.white);
       
    }

   private bool OnSlope()
    {
        RaycastHit2D ray = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), Vector2.right,0.5f, groundLayer);
        RaycastHit2D ray1 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, -0.52f, 0), Vector2.left,0.5f, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.right,0.5f, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0, 0.52f, 0), Vector2.left,0.5f, groundLayer);

       
        if(ray.collider != null)
        {
            slopeAngle = Vector2.Angle(ray.normal, Vector2.up);
        }

        if (ray1.collider != null)
        {
            slopeAngle = Vector2.Angle(ray1.normal, Vector2.up);
          
        }
      

        return ray.collider != null || ray1.collider != null || ray2.collider != null || ray3.collider != null;


    }
}
