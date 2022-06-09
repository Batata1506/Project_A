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
    private RaycastHit2D[] ray = new RaycastHit2D[6]; 
    private float[] slopeAngle;
    // Start is called before the first frame update
    private void Awake()
    {
        body = player.GetComponent<Rigidbody2D>();
        circleCollider = player.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        print(OnSlope());
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(circleCollider.bounds.center - new Vector3(0.25f,0,0), new Vector2(0,-0.5f), Color.red);
        Debug.DrawRay(circleCollider.bounds.center +new Vector3(0.25f, 0, 0), new Vector2(0, -0.5f), Color.red);
        Debug.DrawRay(circleCollider.bounds.center - new Vector3(0.6f, 0, 0), new Vector2(0, -0.20f), Color.red);
        Debug.DrawRay(circleCollider.bounds.center + new Vector3(0.6f, 0, 0), new Vector2(0, -0.20f), Color.red);
        Debug.DrawRay(circleCollider.bounds.center - new Vector3(0.5f, 0, 0), new Vector2(0, -0.25f), Color.red);
        Debug.DrawRay(circleCollider.bounds.center + new Vector3(0.5f, 0, 0), new Vector2(0, -0.25f), Color.red);
    }

   private bool OnSlope()
    {
       
         ray[0] = Physics2D.Raycast(circleCollider.bounds.center - new Vector3(0.25f, 0, 0), Vector2.down, 0.5f, groundLayer);
         ray[1] = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.25f, 0, 0), Vector2.down, 0.5f, groundLayer);
          ray[2] = Physics2D.Raycast(circleCollider.bounds.center - new Vector3(0.5f, 0, 0), Vector2.down, 0.25f, groundLayer);
        ray[3] = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.5f, 0, 0), Vector2.down, 0.25f, groundLayer);
        ray[4] = Physics2D.Raycast(circleCollider.bounds.center - new Vector3(0.6f, 0, 0), Vector2.down, 0.20f, groundLayer);
         ray[5] = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.6f, 0, 0), Vector2.down, 0.20f, groundLayer);

        /*
        for(int i = 0; i < slopeAngle.Length; i++)
        {
            slopeAngle[i] = Vector2.Angle(ray[i].normal, Vector2.up);
        }
        */

            for(int i = 0; i < ray.Length; i++)
            return ray[i].collider != null;

        return false ; 
    }
}
