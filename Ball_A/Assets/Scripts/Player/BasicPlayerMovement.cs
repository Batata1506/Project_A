using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private float horizontalMovement;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;
    private Animator anim;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //Initialising body and boxcollider to the player(whoever is attached to this script)
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {

    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        if (horizontalMovement > 0)
        {
            MoveRight();
        }
        else if (horizontalMovement < 0)
        {
            MoveLeft();
        }

        if(Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
            anim.SetTrigger("jump");
        }
        if(body.velocity.y  != 0 && IsGrounded() == false)
        {
            body.rotation = 0;
            body.freezeRotation = true;
        }
        else if(body.velocity.y == 0 || IsGrounded())
        {
            body.freezeRotation = false;
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if(body.velocity.x != 0)
        {
            anim.SetBool("run", true);
        }
        else if(body.velocity.x == 0)
        {
            anim.SetBool("run", false);
        }
    }

    private void MoveRight()
    {
        body.AddForce(new Vector2(movementSpeed, body.velocity.y));
    }
    private void MoveLeft()
    {
        body.AddForce(new Vector2(-movementSpeed, body.velocity.y));
    }
    
    private void Jump()
    {
        body.AddForce(new Vector2(body.velocity.x, jumpHeight));
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(circleCollider.bounds.center, circleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycast.collider != null;
    }
}
