using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopExit : MonoBehaviour
{
    [SerializeField] private Transform Q3;
    [SerializeField] private Transform Q4;
    [Header("Edge colliders")]
    [SerializeField] private EdgeCollider2D Q3E;
    [SerializeField] private EdgeCollider2D Q4E;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Q3.gameObject.layer = LayerMask.NameToLayer("ground");
            Q4.gameObject.layer = LayerMask.NameToLayer("ground");
            Q3E.isTrigger = false;
            Q4E.isTrigger = false;

            //body.isKinematic = true;

            // Here you write the code that allows the character to move
            // in a circle, e.g. via a bezier curve, at the currentSpeed
        }
    }
}
