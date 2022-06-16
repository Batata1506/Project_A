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
    [SerializeField] private float minimumSlopeSpeed;
    [SerializeField] private float minimumSlopeCatchSpeed;
    private bool minSlopeSpeedReached;
    private float slopeJumpFix; //When player holds jump on slope < 45 angle and is moving then they can get extreme heights(bunny hop) //Also works for >=45 degree angles(running and jumping + holding space on them)
    private SlopeDetection slopeDetect;
    public bool enteringSlope; //Same as onSlope tbh says when entering slope or on slope

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //Initialising body and boxcollider to the player(whoever is attached to this script)
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        slopeDetect = GetComponent<SlopeDetection>();
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

        Debug.DrawRay(circleCollider.bounds.center + new Vector3(-0.3f, 0, 0), new Vector3(0, -0.56f, 0), Color.gray);
        Debug.DrawRay(circleCollider.bounds.center + new Vector3(0.3f, 0, 0), new Vector3(0, -0.56f, 0), Color.gray);
        JumpLessWhenLetGoOfSpace();

        if(slopeDetect.OnLoop())
        {
            Xpos = -Input.GetAxis("Horizontal");
        }
        else
        {
            Xpos = Input.GetAxis("Horizontal");
        }

        _move = new Vector2(Xpos, 0);

        SlopeCalculationForMovement();

        // Set animator paras 
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("grounded", slopeDetect.OnSlope() == true);
        anim.SetBool("run", body.velocity.x != 0);

        //Slopes
        //  CalculateSlopeAngle();
        SlopeJumpFix();

        MinSlopeSpeedReached();
    }

    private void MinSlopeSpeedReached()
    {
        if (Mathf.Abs(body.velocity.x) > minimumSlopeSpeed && (IsGrounded() || slopeDetect.OnSlope()))
            minSlopeSpeedReached = true;
        else if (IsGrounded() == false || slopeDetect.OnSlope() == false)
        {
            minSlopeSpeedReached = false;
        }

        //Goes down slope when not meeting requirements
        if(body.velocity.x < minimumSlopeSpeed/2 && slopeDetect.OnSlope() && body.velocity.y < 20)
        {
            body.velocity += new Vector2(0, -0.1f);
        }
    }

    private void SlopeCalculationForMovement()
    {

        if (slopeDetect.OnSlope() == false)
        {
            _move = new Vector2(Xpos, 0);
        }
        else if (slopeDetect.OnSlope() == true && Xpos > 0 && minSlopeSpeedReached == true && body.velocity.y > 0) //Moving up right slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeDetect.slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeDetect.slopeAngle * Mathf.Deg2Rad));
        }
        else if (slopeDetect.OnSlope() == true && Xpos > 0 && body.velocity.y < 0) //Moving down right slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeDetect.slopeAngle * Mathf.Deg2Rad), -Mathf.Sin(slopeDetect.slopeAngle * Mathf.Deg2Rad));
        }
        else if (slopeDetect.OnSlope() == true && Xpos < 0 && body.velocity.y < 0) //Moving down left slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeDetect.slopeAngle * Mathf.Deg2Rad), -Mathf.Sin(slopeDetect.slopeAngle * Mathf.Deg2Rad));
        }
        else if (slopeDetect.OnSlope() == true && Xpos < 0 && minSlopeSpeedReached == true && body.velocity.y > 0) //Moving up left slopes
        {
            _move = new Vector2(Xpos * Mathf.Cos(slopeDetect.slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeDetect.slopeAngle * Mathf.Deg2Rad));
        }

        if (Xpos > 0.1f)
        {
            transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f); ;
        }
        else if (Xpos < -0.1f)
        {
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
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


        // jump rotayion 
        if (JumpRotationFreeze()) // || slopeDetect.OnSlope() = false
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
        if (slopeDetect.OnSlope() == true && Mathf.Abs(body.velocity.x + body.velocity.y) > minimumSlopeCatchSpeed && Input.GetAxis("Horizontal") != 0)
        {
            body.AddForce(_move * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        /*
        if(Mathf.Abs(body.velocity.x) > _maxSpeed && IsGrounded())
        {
            //Player caps at max speed
            //body.velocity = new Vector2(Mathf.Sign(body.velocity.x * _maxSpeed) * Mathf.Cos(slopeAngle * Mathf.Deg2Rad), body.velocity.y * Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
            movementSpeed -= 0.05f;
        }
        */
       
    }
   
    private void Jump()
    {   //Entering slope same as onSlope
        if (slopeDetect.slopeAngle >= 54 && slopeDetect.OnSlope() == true && slopeDetect.slopeAngle <= 80) //Over 55 degree slopes
        {
            if (body.velocity.y >= 8)
            {
                body.velocity = new Vector2((body.velocity.x * 1) * -Mathf.Tan(slopeDetect.slopeAngle * Mathf.Deg2Rad), jumpHeight + (body.velocity.x * 0.5f)); //Going up slopes and moving at fast speed
                anim.SetTrigger("jump");
            }
            else if (body.velocity.y < -12 && slopeDetect.OnSlope()) //Going down
            {
                body.velocity = new Vector2((Mathf.Abs(body.velocity.x + body.velocity.y) * 0.3f) * Mathf.Tan(slopeDetect.slopeAngle * Mathf.Deg2Rad) * -0.6f - 3, jumpHeight);
                anim.SetTrigger("jump");
            }
            else
            {
                body.velocity = new Vector2(Mathf.Tan(slopeDetect.slopeAngle * Mathf.Deg2Rad) - 30f, jumpHeight); //Going up slowly
                anim.SetTrigger("jump");
            }
        }
        else if (slopeDetect.slopeAngle < 54 && slopeDetect.OnSlope() == true)
        {
            if (slopeJumpFix > 0.1 && body.velocity.y >= 0)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeDetect.slopeAngle * Mathf.Deg2Rad) + (body.velocity.x), jumpHeight + Mathf.Abs(body.velocity.x + body.velocity.y) * 0.5f);
                anim.SetTrigger("jump");
            }
            else if (slopeJumpFix > 0.1 && body.velocity.y < 0)
            {
                body.velocity = new Vector2(Mathf.Tan(slopeDetect.slopeAngle * Mathf.Deg2Rad) + (body.velocity.x * 1.2f), jumpHeight + Mathf.Abs(body.velocity.x + body.velocity.y) * 0.25f);
                anim.SetTrigger("jump");
            }
        }
        else if (slopeDetect.slopeAngle > 80 && slopeDetect.slopeAngle < 100 && slopeDetect.OnSlope() && IsGrounded() == false)
        {
            if(body.velocity.y > 5)
            {
                body.velocity = new Vector2(jumpHeight * (Mathf.Sign(body.rotation)) + body.velocity.y * 0.4f + Mathf.Abs(body.velocity.x * 2f), body.velocity.y);
                anim.SetTrigger("jump");
            }
            else if(body.velocity.y < 5) //For efficiency could put this condition in first if statement and * equation by Mathf.Sign(body.velocity.y) but it would be less easy to understand
            {
                body.velocity = new Vector2(jumpHeight * -(Mathf.Sign(body.rotation)) + body.velocity.y * 0.4f + Mathf.Abs(body.velocity.x * 2f), body.velocity.y);
                anim.SetTrigger("jump");
            }
        }
        else if (IsGrounded() && enteringSlope == false)
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");

        }
        coolDown = 0;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(-0.3f, 0, 0), Vector2.down, 0.56f, groundLayer);
        RaycastHit2D raycast1 = Physics2D.Raycast(circleCollider.bounds.center + new Vector3(0.3f, 0, 0), Vector2.down, 0.56f, groundLayer);

        if (raycast.collider != null && raycast1.collider == null || raycast.collider == null && raycast1.collider != null)
        {
            enteringSlope = true; //When entering downward slopes
        }
        else
        {
            enteringSlope = false;
        }

        return raycast.collider != null || raycast1.collider != null;
    }

    private void SlopeJumpFix() //When player holds jump on slope < 45 angle and is moving then they can get extreme heights(bunny hop) //Also works for >=45 degree angles(running and jumping + holding space on them)
    {
        if(slopeDetect.OnSlope())
        {
            slopeJumpFix += Time.deltaTime;
        }
        else
        {
            slopeJumpFix = 0;
        }
    }

    private bool JumpRotationFreeze()
    {
        RaycastHit2D boxcast = Physics2D.BoxCast(circleCollider.bounds.center, circleCollider.bounds.size + new Vector3(0.1f, 0.1f, 0), 0, Vector2.down, 0.01f, groundLayer);
        return boxcast.collider == null;
    }
    /*
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
    */
}
