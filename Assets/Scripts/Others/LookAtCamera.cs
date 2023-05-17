using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = gameObject.scene.GetFirstMainCameraInScene();
    }
    
    private void LateUpdate()
    {
        var _camRot = cam.transform.rotation;
        transform.LookAt(transform.position + _camRot * Vector3.forward, _camRot * Vector3.up);
    }
}
