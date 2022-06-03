using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loops : MonoBehaviour

{
    [SerializeField] private float minimumAttachSpeed; //The minimum speed required on loops
    private float currentSpeed; //Players current speed
    private Rigidbody2D body;

    private bool LoopAttachmentCheck()
    {
        return minimumAttachSpeed <= currentSpeed;
    }

    private void Update()
    {
        currentSpeed = body.velocity.x;
        if (LoopAttachmentCheck() == true)
        {
            body.isKinematic = true;
        }

        else
        {
            body.isKinematic = false;
        }
    }
}
