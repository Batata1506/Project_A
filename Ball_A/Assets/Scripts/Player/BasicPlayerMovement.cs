using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _maxSpeed = 50f;
    [SerializeField] private float jumpCooldown ;
    private Collider2D circleCollider;
    private Rigidbody2D body;
    private Vector2 _move;
    private Animator anim;
    private float Xpos;
    private float coolDown = Mathf.Infinity;

    [Header("Slopes")]
    [SerializeField] public float slopeAngle; // only serialized for debugging
    [SerializeField] private float maxClimableAngle;
    public bool onSlope;
    [SerializeField] private float minimumSlopeSpeed;
    [SerializeField] private float minimumSlopeCatchSpeed;
    private bool minSlopeSpeedReached;
    private float slopeJumpFix; //When player holds jump on slope < 45 angle and is moving then they can get extreme heights(bunny hop) //Also works for >=45 degree angles(running and jumping + holding space on them)



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
    //Always initialise physics in Update but implement in FixedUpdate
    private void Update()
    {
        //  Debug.DrawLine(circleCollider.bounds.center +  new Vector3(0, -0.5f, 0), circleCollider.bounds.center + new Vector3(0.35f, -0.5f, 0)); //For slopes
        // Debug.DrawRay(circleCollider.bounds.center + new Vector3(0f, -0.52f, 0), Vector2.right, Color.blue); //For slopes

        JumpLessWhenLetGoOfSpace();

        Xpos = Input.GetAxis("Horizontal");
        if (onSlope == false)
        {
            _move = new Vector2(Xpos, 0);
        }
        else if (onSlope == true && Xpos > 0 && minSlopeSpeedReached == true) //Moving up slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
        }
        else if (onSlope == true && Xpos < 0) //Moving down slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeAngle * Mathf.Deg2Rad), -Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
        }


        if (Xpos > 0.1f)
        {
            transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f); ;
        }
        else if (Xpos < -0.1f)
        {
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        // Set animator paras 
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("grounded", onSlope == true);
        anim.SetBool("run", body.velocity.x != 0);

        //Slopes
        CalculateSlopeAngle();

        if (body.velocity.x > minimumSlopeSpeed && (IsGrounded() || onSlope))
            minSlopeSpeedReached = true;
        else if (IsGrounded() == false || onSlope == false)
        {
            minSlopeSpeedReached = false;
        }
    }

    private void JumpLessWhenLetGoOfSpace()
    {
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 1.5f);
        }
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        
        Move();

        coolDown += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }


        // jump rotaion 
        if (!IsGrounded()) // || onSlope = false
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
        //print(body.velocity.x + body.velocity.y);
        body.AddForce(_move*movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
        if (onSlope == true && body.velocity.x + body.velocity.y > minimumSlopeCatchSpeed && Input.GetAxis("Horizontal") != 0)
        {
            body.AddForce(_move * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        if(Mathf.Abs(body.velocity.x) > _maxSpeed && IsGrounded())
        {
            //Player caps at max speed
            //body.velocity = new Vector2(Mathf.Sign(body.velocity.x * _maxSpeed) * Mathf.Cos(slopeAngle * Mathf.Deg2Rad), body.velocity.y * Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
            movementSpeed -= 0.05f;
        }
       
    }
   
    private void Jump()
    {   //FOR 45degrees+ slopes
        if(slopeAngle >= 45)
        {
            if (onSlope == true && slopeAngle != 90 && body.velocity.x > movementSpeed * 0.25f && slopeJumpFix > 1)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * -(body.velocity.x * 1.5f), jumpHeight);
                anim.SetTrigger("jump");

            }
            else if (onSlope == true && slopeAngle != 90 && body.velocity.x <= 0)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * (body.velocity.x * 1.5f), jumpHeight);
                anim.SetTrigger("jump");
            }
            else if (onSlope == true && slopeAngle != 90)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * -1f, jumpHeight);
                anim.SetTrigger("jump");
            }
        }
        if (slopeAngle < 45 && onSlope == true)
        {
            if (onSlope == true && slopeAngle != 90 && body.velocity.x > movementSpeed * 0.25f && slopeJumpFix > 0.5)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * (body.velocity.x * 3f), jumpHeight * Mathf.Abs(body.velocity.x * 0.1f));
                anim.SetTrigger("jump");
            }
            else if (onSlope == true && slopeAngle != 90 && body.velocity.y <= 0)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * (body.velocity.x * 4f), jumpHeight);
                anim.SetTrigger("jump");
            }
            else if (onSlope == true && slopeAngle != 90)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * -1f, jumpHeight);
                anim.SetTrigger("jump");
            }
        }

        else if (IsGrounded() && onSlope == false)
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");

        }
        coolDown = 0;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(circleCollider.bounds.center,circleCollider.bounds.size,0,Vector2.down,0.03f, groundLayer);

        return raycast.collider != null;
    }

    private void CalculateSlopeAngle()
    {
        //RaycastHit2D hit = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.35f, -0.5f, 0), Vector2.down, 0.03f, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0f, -0.52f, 0), Vector2.right, 0.5f, groundLayer); //Change groundLayer to slopeLayer
        if (hit)
        {
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            //print(slopeAngle);
            onSlope = true;
            slopeJumpFix += Time.deltaTime;
        }
        else
        {
            slopeJumpFix = 0;
            onSlope = false;
        }
    }
}
