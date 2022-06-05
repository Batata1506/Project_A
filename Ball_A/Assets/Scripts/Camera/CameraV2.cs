using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraV2 : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Rigidbody2D body;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        body = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movePosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
}
