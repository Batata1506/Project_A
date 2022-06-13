using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerNew : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] public float damping;
    private Rigidbody2D body;
    private Vector3 initialOffset;

    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        body = player.GetComponent<Rigidbody2D>();
        initialOffset = offset;
    }

    private void FixedUpdate()
    {
        //The lower damping is the faster the camera moves(at 0.1 it tps)
        if (Mathf.Abs(body.velocity.x) > 12)
        {
            damping -= 0.002f;
            if (damping <= 0.2f)
            {
                damping = 0.2f;
            }
            offset = new Vector3(4.5f * -Mathf.Sign(player.localScale.x), offset.y, offset.z);
        }
        else if (Mathf.Abs(body.velocity.x) > 3)
        {
            damping -= 0.002f;
            if (damping <= 0.3f)
            {
                damping = 0.3f;
            }
            offset = new Vector3(3 * -Mathf.Sign(player.localScale.x), offset.y, offset.z);
        }
        else
        {
            offset.x = initialOffset.x;
        }

        if (Mathf.Abs(body.velocity.y) > 5)
        {
            damping -= 0.002f;
            if (damping <= 0.2f)
            {
                damping = 0.2f;
            }
            offset = new Vector3(offset.x, 3.14f * Mathf.Sign(body.velocity.y), offset.z);
        }
        else
        {
            offset.y = initialOffset.y;
        }

        if (offset == initialOffset)
        {
            damping = 0.4f;
        }
        Vector3 movePosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);

        //Catchs up to the player if player is far from the camera(tested by falling off map and camera chases player)
        if (transform.position.x - player.position.x > 8 || transform.position.y - player.position.y > 8)
        {
            damping -= 0.05f;
            if(damping <= 0.15f)
            {
                damping = 0.15f;
            }
        }
    }
   
}
