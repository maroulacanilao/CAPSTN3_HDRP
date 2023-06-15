using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mCam;
    private Camera cam
    {
        get
        {
            if(mCam == null) mCam = Camera.main;
            
            if (!mCam.transform.gameObject.activeInHierarchy)
            {
                mCam = gameObject.scene.GetFirstMainCameraInScene(false);
            }
            return mCam;
        }
    }

    private void Awake()
    {
        mCam = Camera.main;
    }
    
    private void LateUpdate()
    {
        var _camRot = cam.transform.rotation;
        transform.LookAt(transform.position + _camRot * Vector3.forward, _camRot * Vector3.up);
    }
}
