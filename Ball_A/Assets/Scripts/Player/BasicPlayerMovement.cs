using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private Vector2 _move;
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private LayerMask groundLayer;
    private float _maxSpeed =  220f;
    private Animator anim;
    private float horiDirection;
    private bool  direction ;

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
        if(Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
            anim.SetTrigger("jump");

             if (Input.GetKey(KeyCode.F) )
        {
            body.gravityScale = 5f;
         
            if (IsGrounded())
            {
                body.gravityScale = 1f;
                Jump();
                anim.SetTrigger("jump");
            }
          
        }
           
        }

        if ( IsGrounded() == false)
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
        _move = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        horiDirection = Input.GetAxis("Horizontal");
        direction = (body.velocity.x > 0f && horiDirection < 0f) || (body.velocity.x < 0f && horiDirection > 0f);

        if (Mathf.Abs(horiDirection) < 0.4f || direction)
        {
            anim.SetBool("run", true);
        }
        else anim.SetBool("run", false);


    }

    private void Move()
    {
         body.AddForce(_move*movementSpeed*Time.deltaTime, ForceMode2D.Force);
        if(Mathf.Abs(body.velocity.x) > _maxSpeed)
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x* _maxSpeed), body.velocity.y  );
        

       // body.velocity = new Vector2(_move * movementSpeed * Time.deltaTime, body.velocity.y);
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
