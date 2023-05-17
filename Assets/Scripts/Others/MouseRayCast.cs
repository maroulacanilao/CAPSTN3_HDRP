using System.Collections.Generic;
using BaseCore;
using CustomHelpers;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRayCast : Singleton<MouseRayCast>
{
    [SerializeField] private Transform player;
    [SerializeField] private float nearDistance = 2f;
    [SerializeField] private LayerMask groundLayer;
    public bool isNearPlayer { get; private set; }
    public Vector3 worldPosition { get; private set; }
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = gameObject.scene.GetFirstMainCameraInScene();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void FixedUpdate()
    {
        var _position = Mouse.current.position.ReadValue();

        var _ray = mainCamera.ScreenPointToRay(_position);

        if (!Physics.Raycast(_ray, out var hitInfo, groundLayer))
        {
            isNearPlayer = false;
            worldPosition = Vector3.negativeInfinity;
            return;
        }
        
        worldPosition = hitInfo.point;
        var _distance = Vector3.Distance(player.position, worldPosition);
        isNearPlayer = _distance <= nearDistance;
    }

    private void IsOverUI()
    {
        
    }
    
    private void IsNearPlayer()
    {
        
    }
}
