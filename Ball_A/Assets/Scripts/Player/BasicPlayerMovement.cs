using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _maxSpeed = 50f;
    [SerializeField] private float jumpCooldown ;
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private Vector2 _move;
    private Animator anim;
    private float Xpos;
    private float coolDown = Mathf.Infinity;

 

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
    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        Xpos = Input.GetAxis("Horizontal");
        _move = new Vector2(Xpos, 0);
        if (Xpos > 0.1f)
        {
            transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f); ;
        }
        else if (Xpos < -0.1f)
        {
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        // Set animator paras 
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("run", body.velocity.x != 0);
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        Move();

        coolDown += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && coolDown > jumpCooldown)
        {
            Jump();
        }


        // jump rotaion 
        if (!isGrounded())
        {
            body.rotation = 0;
            body.freezeRotation = true;
            body.drag = 1;
        }
        else
        {
            body.freezeRotation = false;
            body.drag = 2;
        }
    }

    

    private void Move()
    {
         body.AddForce(_move*movementSpeed*Time.deltaTime, ForceMode2D.Impulse);
        if(Mathf.Abs(body.velocity.x) > _maxSpeed)
        {
            //Player caps at max speed
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x * _maxSpeed), body.velocity.y);
        }
       
    }
   
    private void Jump()
    {
      
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");
            
        }
        coolDown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(circleCollider.bounds.center,circleCollider.bounds.size,0,Vector2.down,0.03f, groundLayer);
 
        return raycast.collider != null;
    }

   
}
