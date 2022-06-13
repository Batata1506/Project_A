using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QToggle : MonoBehaviour
{
    private CameraFollowCharacter CamScript1;
    private CameraFollowPlayerNew CamScript2;
    [SerializeField] private Transform Camera;
    private void Start()
    {
        CamScript1 = Camera.GetComponent<CameraFollowCharacter>();
        CamScript2 = Camera.GetComponent<CameraFollowPlayerNew>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeCameras();
        }
    }

    private void ChangeCameras()
    {
        if(CamScript1.enabled == true)
        {
            CamScript1.enabled = false;
            CamScript2.enabled = true;
        }
        else if (CamScript2.enabled == true)
        {
            CamScript2.enabled = false;
            CamScript1.enabled = true;
        }
    }
}
