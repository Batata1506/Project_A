using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFollowCameraX : MonoBehaviour
{
    private float initialYPos;
    private void Start()
    {
        initialYPos = transform.position.y;
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, initialYPos);
    }
}


