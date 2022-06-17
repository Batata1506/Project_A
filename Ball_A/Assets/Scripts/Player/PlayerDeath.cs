using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
   [SerializeField] private GameObject respawn;
    private Animator anim;
    private Rigidbody2D body;
    private CircleCollider2D circleCollider;
    [SerializeField] private LayerMask deathPlane;
    [SerializeField] private Transform cam;
    private CameraFollowPlayerNew camScript;
    private CameraFollowCharacter camScript2;
    private float cameraNumber;
  
    private void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        camScript = cam.GetComponent<CameraFollowPlayerNew>();
        camScript2 = cam.GetComponent<CameraFollowCharacter>();
    }

    // Update is called once per frame
    public void Death() // method that casue the death 
    {
            anim.SetTrigger("death");
            anim.SetBool("respawn", true);

        body.position = respawn.GetComponent<Rigidbody2D>().position;

    }

    private void Update()
    {
        if(AboutToDie() && camScript.enabled)
        {
            camScript.enabled = false;
            cameraNumber = 1;
        }
        else if(AboutToDie() && camScript2.enabled)
        {
            camScript2.enabled = false;
            cameraNumber = 2;
        }
        else if(cameraNumber == 1 && AboutToDie() == false)
        {
            camScript.enabled = true;
            cameraNumber = 0;
        }
        else if (cameraNumber == 2 && AboutToDie() == false)
        {
            camScript2.enabled = true;
            cameraNumber = 0;
        }
    }

    private bool AboutToDie() //Uses raycast to check if player is about to fall to his death
    {
        RaycastHit2D raycast = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, 6, deathPlane);
        return raycast.collider != null; 
    }

}
