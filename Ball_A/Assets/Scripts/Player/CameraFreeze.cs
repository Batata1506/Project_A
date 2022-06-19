using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    private CameraFollowPlayerNew camScript;
    private CameraFollowCharacter camScript2;
    private float cameraNumber;
    private PlayerDeath deathScript;
    [SerializeField] private Transform cam;
    private CircleCollider2D circleCollider;
    [SerializeField] private LayerMask wall;

    private void Awake()
    {
        camScript = cam.GetComponent<CameraFollowPlayerNew>();
        camScript2 = cam.GetComponent<CameraFollowCharacter>();
        deathScript = GetComponent<PlayerDeath>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        FreezeCameraWhenAboutToFallInDeathPut();
    }

    private void FreezeCameraWhenAboutToFallInDeathPut()
    {
        if (deathScript.AboutToDie() && camScript.enabled)
        {
            camScript.enabled = false;
            cameraNumber = 1;
        }
        else if (deathScript.AboutToDie() && camScript2.enabled)
        {
            camScript2.enabled = false;
            cameraNumber = 2;
        }
        else if (cameraNumber == 1 && deathScript.AboutToDie() == false)
        {
            camScript.enabled = true;
            cameraNumber = 0;
        }
        else if (cameraNumber == 2 && deathScript.AboutToDie() == false)
        {
            camScript2.enabled = true;
            cameraNumber = 0;
        }
    }

    private void FreezeCameraWhenAtBeginningOrEndWall()
    {
        if (deathScript.AboutToDie() && camScript.enabled)
        {
            camScript.enabled = false;
            cameraNumber = 1;
        }
        else if (deathScript.AboutToDie() && camScript2.enabled)
        {
            camScript2.enabled = false;
            cameraNumber = 2;
        }
        else if (cameraNumber == 1 && deathScript.AboutToDie() == false)
        {
            camScript.enabled = true;
            cameraNumber = 0;
        }
        else if (cameraNumber == 2 && deathScript.AboutToDie() == false)
        {
            camScript2.enabled = true;
            cameraNumber = 0;
        }
    }
    private bool NearEndOrStartWall() //Uses raycast to check if player is about to fall to his death
    {
        RaycastHit2D raycast = Physics2D.Raycast(circleCollider.bounds.center, Vector2.right, 6, wall);
        RaycastHit2D raycast1 = Physics2D.Raycast(circleCollider.bounds.center, Vector2.left, 6, wall);
        return raycast.collider != null || raycast1.collider != null;
    }
}
