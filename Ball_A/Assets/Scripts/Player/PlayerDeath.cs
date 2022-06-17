using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Obstacles obstacle;
   [SerializeField] private GameObject respawn;
    private Rigidbody2D playerPos;
  
    private void Start()
    {
        obstacle = GetComponent<Obstacles>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (obstacle.IsTouchingBall())
        {
            obstacle.Death();
            playerPos.position = respawn.GetComponent<Rigidbody2D>().position;  
        }
        
    }


   
}
