using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraZoom : MonoBehaviour
{
    public bool ZoomActive;

    public Vector3[] Target;

    public Camera cam;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(ZoomActive)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3, speed);
            cam.transform.position = Vector3.Lerp(cam.transform.position, Target[1], speed);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, speed);
            cam.transform.position = Vector3.Lerp(cam.transform.position, Target[1], speed);
        }
    }
}
