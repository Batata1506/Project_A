using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private CircleCollider2D ballCollider;
    private EdgeCollider2D rockCollider;
    private EdgeCollider2D stickCollider;
    private Rigidbody2D ball;
    private Animator anim;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject respawn;
    // Start is called before the first frame update
  
    private void Awake()
    {
        ball = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<CircleCollider2D>();    
        rockCollider = rock.GetComponent<EdgeCollider2D>();
        stickCollider = stick.GetComponent<EdgeCollider2D>(); 
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }

    private void Death ()  
    {
        if (ballCollider.IsTouching(rockCollider) || ballCollider.IsTouching(stickCollider))
        {
            stickCollider.enabled = false;
            rockCollider.enabled = false;
            anim.SetTrigger("death");
          ball.position = respawn.GetComponent<Rigidbody2D>().position;
            anim.SetBool("respawn", true);

        }
    }

   
}
