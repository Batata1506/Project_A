using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCharacter : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float cdTimer;
    private Rigidbody2D body;
    private bool changingDirections => body.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0 || body.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0;
    private float lookAhead;
    [Header("Y Camera Movement")]
    [SerializeField] private float offSet;
    [SerializeField] private float dampingMultiplier;
    private float damping;
    private float yCamera;
    private float yVelocity;
    [SerializeField] private float dampingChange;
    private BasicPlayerMovement coreMove;

    private void Start()
    {
        body = player.GetComponent<Rigidbody2D>();
        coreMove = player.GetComponent<BasicPlayerMovement>();
    }
    private void FixedUpdate()
    {
        float offsetPosition = player.position.y + offSet;
        float yDamping = 0;
        
                        //was 0.15f
            dampingChange = 0.15f * Mathf.Abs(body.velocity.x + body.velocity.y);

        float damping = (Mathf.Abs(dampingMultiplier - Mathf.Abs((player.position.y + offSet) - player.position.y) - dampingChange) -yDamping)+ 0.5f;

        if (Mathf.Abs(transform.position.y - offsetPosition) > 0 && Mathf.Abs(transform.position.y - offsetPosition) < 2)
            damping = 0.5f;

        if (Mathf.Abs(transform.position.y - offsetPosition) > 3 && Mathf.Abs(transform.position.y - offsetPosition) < 4)
            damping = 0.5f;
        //Y camera
        bool falling = false;


        if (coreMove.onSlope)
        {
            if(Mathf.Abs(body.velocity.y) * Mathf.Sin(coreMove.slopeAngle * Mathf.Deg2Rad) > 15)
            {
                damping = 0.1f;
                if (body.velocity.y < -0.5 && transform.position.y - offsetPosition > 4)
                {
                    falling = true;
                    offSet = -2f;
                }
            }
        }


        //Same as above except when not on slope
        if (transform.position.y - offsetPosition > 4f && Mathf.Abs(body.velocity.y) > 23 && Mathf.Abs(body.velocity.x) < 20 && coreMove.onSlope == false || Mathf.Abs(transform.position.y - offsetPosition) > 8f)
        {
            damping = 0.1f;
            if (body.velocity.y < -0.5 && transform.position.y - offsetPosition > 4)
            {
                falling = true;
                offSet = -1.5f;
            }
        }
        if (body.velocity.y < -0.5)
        {
            if(falling == false)
            offSet = 0;
        }
        else if(falling == false)
        {

            offSet = 3.41f;
        }

        yCamera = Mathf.SmoothDamp(transform.position.y, offsetPosition, ref yVelocity, damping);
        cdTimer = 0;

        if(changingDirections)
        {
            cdTimer += Time.deltaTime;
                if(cdTimer > 0)
            {
                transform.position = new Vector3(player.position.x + lookAhead, yCamera, transform.position.z);
                lookAhead = Mathf.Lerp(lookAhead, (25 * -player.localScale.x), Time.deltaTime * cameraSpeed);
                cdTimer = 0;
            }
        }
        else
        {
            transform.position = new Vector3(player.position.x + lookAhead, yCamera, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * -player.localScale.x), Time.deltaTime * (cameraSpeed + 0.5f));
        }
    }
}
