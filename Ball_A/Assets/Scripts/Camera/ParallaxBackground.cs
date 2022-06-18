using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField] private Vector2 parallexEffectMultiplier;
    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        //delta = change
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallexEffectMultiplier.x, deltaMovement.y * parallexEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x + 20) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y);
        }
    }
}
