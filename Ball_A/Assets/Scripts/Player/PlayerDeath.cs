using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
   [SerializeField] private GameObject respawn;
    private Animator anim;
    private Rigidbody2D body;
    private CircleCollider2D circleCollider;
    [SerializeField] private LayerMask deathPlane;
  
    private void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();;
    }

    // Update is called once per frame
    public void Death() // method that casue the death 
    {
            anim.SetTrigger("death");
            anim.SetBool("respawn", true);

        body.position = respawn.GetComponent<Rigidbody2D>().position;

    }
    public bool AboutToDie() //Uses raycast to check if player is about to fall to his death
    {
        RaycastHit2D raycast = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, 6, deathPlane);
        return raycast.collider != null; 
    }

}
