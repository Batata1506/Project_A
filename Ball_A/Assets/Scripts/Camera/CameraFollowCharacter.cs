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

    private void Start()
    {
        body = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        cdTimer = 0;

        if(changingDirections)
        {
            cdTimer += Time.deltaTime;
                if(cdTimer > 0)
            {
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
                lookAhead = Mathf.Lerp(lookAhead, (25 * -player.localScale.x), Time.deltaTime * cameraSpeed);
                cdTimer = 0;
            }
        }
        else
        {
            transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * -player.localScale.x), Time.deltaTime * (cameraSpeed + 0.5f));
        }
    }
}
