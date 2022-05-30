using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private Animator anim;

    [Header("Player Attributes")]
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundLinearDrag;
    [SerializeField] private float airLinearDrag;
    [SerializeField] private float maxJumpHeight;
    private float horizontalInput;
    private bool changingDirections => (body.velocity.x > 0f && horizontalInput < 0f) || (body.velocity.x < 0f && horizontalInput > 0f);


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
        Move();

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
            anim.SetTrigger("jump");
        }

        //Rotations
        if (body.velocity.y != 0 && IsGrounded() == false)
        {
            body.rotation = 0;
            body.freezeRotation = true;
        }
        else if (body.velocity.y == 0 || IsGrounded())
        {
            body.freezeRotation = false;
        }

        //Linear Drag
        if(IsGrounded())
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        horizontalInput = GetInput().x;
        if (body.velocity.x != 0)
        {
            anim.SetBool("run", true);
        }
        else if (body.velocity.x == 0)
        {
            anim.SetBool("run", false);
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        body.AddForce(new Vector2(horizontalInput, 0f) * acceleration);

        if (Mathf.Abs(body.velocity.x) > maxMoveSpeed)
        {
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxMoveSpeed, body.velocity.y);
        }
    }
    private void ApplyGroundLinearDrag() //Deceleration
    {
        if(Mathf.Abs(horizontalInput) < 0.4f && IsGrounded()|| changingDirections && IsGrounded())
        {
            body.drag = groundLinearDrag;
        }
        else
        {
            body.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag() //Deceleration in air
    {
        body.drag = airLinearDrag;
    }
    private void Jump()
    {
        body.AddForce(new Vector2(body.velocity.x, jumpHeight), ForceMode2D.Impulse);
        //Clamps jump height
        if (Mathf.Abs(body.velocity.x) > maxJumpHeight)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * maxJumpHeight);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, 0.75f, groundLayer);
        return raycast.collider != null;

    }
}
