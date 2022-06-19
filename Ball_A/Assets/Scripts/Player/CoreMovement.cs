using JetBrains.Annotations;
using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CoreMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] public float jumpHeight = 100f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _maxSpeed = 50f;
    [SerializeField] private float jumpCooldown ;
    [SerializeField] public float bounceCooldown;
    public CircleCollider2D circleCollider;
    public Rigidbody2D body;
    private Vector2 _move;
    public Animator anim;
    private float Xpos;
    private float coolDown = Mathf.Infinity;
    private BounceAbility bounceAbility;
    public bool isJumping; // stops player holding m1
    

    
   

 

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //Initialising body and boxcollider to the player(whoever is attached to this script)
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
      bounceAbility =FindObjectOfType<BounceAbility>();  
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
        //StartCoroutine(BounceDelay2());
        Move();
        if (Input.GetButton("Jump") && coolDown > jumpCooldown)
        {
            Jump(); 
            isJumping = true;
        }
        if ( bounceAbility.coolDownB > bounceCooldown)
        {
            bounceAbility.Bounce();
        }


        coolDown += Time.fixedDeltaTime;
        bounceAbility.coolDownB += Time.fixedDeltaTime;
 
        // jump rotaion 
        if (isGrounded())
        {
            body.freezeRotation = false;
            body.drag = 2;
        }
        else
        {
            body.rotation = 0;
            body.freezeRotation = true;
            body.drag = 1;
        }
    }

    

    private void Move()
    {
         body.AddForce(_move*movementSpeed*Time.fixedDeltaTime, ForceMode2D.Impulse);
        if(Mathf.Abs(body.velocity.x) > _maxSpeed)
        {
            //Player caps at max speed
            body.velocity = new Vector2(Mathf.Sign(Xpos * _maxSpeed), body.velocity.y);
        }
       
    }
   
    private void Jump()
    {
        if (isGrounded() )
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");
            isJumping = true;
        }
        coolDown = 0;
    }

  

    public  bool isGrounded()
    {
        
        RaycastHit2D raycast = Physics2D.BoxCast(circleCollider.bounds.center, circleCollider.bounds.size, 0f, Vector2.down, 0.01f, groundLayer);

        return raycast.collider != null;
    }


}
