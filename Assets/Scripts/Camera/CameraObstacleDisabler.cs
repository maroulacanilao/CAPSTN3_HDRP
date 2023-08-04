using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Dungeon;
using Player;
using UnityEngine;

[DefaultExecutionOrder(88)]
public class CameraObstacleDisabler : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Material defaultMaterial, seeThroughMaterial;
    
    private Transform player;
    private Camera mainCamera;
    private float checkInterval = 0.2f;

    private List<GameObject> disabledObjects = new List<GameObject>();

    private void Awake()
    {
        player = PlayerInputController.Instance.transform;
        mainCamera = GetComponent<Camera>();
        
    }
    private IEnumerator StartDisabling()
    {
        while (gameObject.activeInHierarchy)
        {
            DisableObjects();
            ReEnableObjects();

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(StartDisabling());
    }

    private void DisableObjects()
    {
        Vector3 direction = mainCamera.transform.forward;
        float _dist = Vector3.Distance(player.position, mainCamera.transform.position);
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, _dist, obstacleLayer);

        foreach (RaycastHit hit in hits)
        {
            GameObject obstacle = hit.collider.gameObject;
            if (!disabledObjects.Contains(obstacle))
            {
                disabledObjects.Add(obstacle);
                var _obstacle = obstacle.GetComponent<CameraObstacle>();
                if (_obstacle != null)
                {
                    _obstacle.InvisibleMaterial();
                }
            }
        }
    }

    private void ReEnableObjects()
    {
        for (int i = disabledObjects.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = disabledObjects[i];
            if (!IsObstacleBetweenPlayerAndCamera(obstacle))
            {
                var _obstacle = obstacle.GetComponent<CameraObstacle>();
                if (_obstacle != null)
                {
                    _obstacle.ResetMaterial();
                }
                disabledObjects.RemoveAt(i);
            }
        }
    }

    private bool IsObstacleBetweenPlayerAndCamera(GameObject obstacle)
    {
        Vector3 direction = mainCamera.transform.forward;
        float _dist = Vector3.Distance(player.position, mainCamera.transform.position);
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, _dist, obstacleLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == obstacle)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if(player == null) return;
        if(mainCamera == null) return;
        Debug.DrawLine(player.position, mainCamera.transform.position, Color.red);
        Vector3 direction = mainCamera.transform.position - player.position;
        Ray ray = new Ray(player.position, direction);
        Gizmos.DrawRay(ray);
        
    }
}
