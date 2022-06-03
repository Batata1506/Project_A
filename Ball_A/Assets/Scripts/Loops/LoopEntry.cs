using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopEntry : MonoBehaviour
{
    [SerializeField] private Transform Q1;
    [SerializeField] private Transform Q2;
    [Header("Edge colliders")]
    [SerializeField] private EdgeCollider2D Q1E;
    [SerializeField] private EdgeCollider2D Q2E;
    [SerializeField] private Transform player;
    [Header("Player values")]
    private float currentSpeed; //Players current speed
    [SerializeField] private float minimumSpeed; //The minimum speed required on loops
    private Rigidbody2D body;

        private void Start()
    {
        body = player.GetComponent<Rigidbody2D>();

    }

    private bool LoopAttachmentCheck()
    {
        return minimumSpeed <= currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            Q1.gameObject.layer = LayerMask.NameToLayer("ground");
            Q2.gameObject.layer = LayerMask.NameToLayer("ground");
            Q1E.isTrigger = false;
            Q2E.isTrigger = false;

            //body.isKinematic = true;

            // Here you write the code that allows the character to move
            // in a circle, e.g. via a bezier curve, at the currentSpeed
        }
        else
        {
            body.isKinematic = false;
        }
    }
}
