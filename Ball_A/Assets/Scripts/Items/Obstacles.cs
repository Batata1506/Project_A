using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private EdgeCollider2D edgeCollider; // gets the collider of object
    private CircleCollider2D circleCollider; // gets collider of the ball
   [SerializeField] private GameObject player; // gets the player Object
    private PlayerDeath deathScript;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        deathScript = player.GetComponent<PlayerDeath>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Update() // method that casue the death 
    {
        if (IsTouchingBall())
        {
            deathScript.Death();

        }
        
    }

    public bool IsTouchingBall() // method to check that the player is touching an obstacle
    {
        if (edgeCollider != null)
        {
            circleCollider = player.GetComponent<CircleCollider2D>();
            return circleCollider.IsTouching(edgeCollider);
        }
        else if (boxCollider != null)
        {
            circleCollider = player.GetComponent<CircleCollider2D>();
            return circleCollider.IsTouching(boxCollider);
        }
        else return false;
    }
}
