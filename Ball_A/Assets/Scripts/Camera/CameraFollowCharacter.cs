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
    private float yCamera;
    [SerializeField]private float yVelocity;
    [SerializeField] private float maxYVelocity;
    [SerializeField] private float dampingChange;

    private void Start()
    {
        body = player.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float offsetPosition = player.position.y + offSet;
        float yDamping = 0;
        dampingChange = 0.15f * Mathf.Abs(body.velocity.x + body.velocity.y);
        float damping = (Mathf.Abs(dampingMultiplier - Mathf.Abs((player.position.y + offSet) - player.position.y) - dampingChange) -yDamping)+ 0.5f;
        print(damping);

        if (Mathf.Abs(transform.position.y - offsetPosition) > 0 && Mathf.Abs(transform.position.y - offsetPosition) < 2)
            damping = 0.5f;

        if (Mathf.Abs(transform.position.y - offsetPosition) > 3)
            damping = 0.5f;
        //Y camera
        yCamera = Mathf.SmoothDamp(transform.position.y, offsetPosition, ref yVelocity, damping);
        if(body.velocity.y < -0.5)
        {
            offSet = 0;
        }
        else
        {
            offSet = 3.41f;
        }




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
