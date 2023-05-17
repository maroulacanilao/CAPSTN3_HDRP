using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class MovementCollisionDetector : MonoBehaviour
{
    [System.Serializable]
    public class GroundRayInfo
    {
        public Vector3 groundOffset;
        public float groundDistance;
    }

    [Header("Ground")]
    [SerializeField] private GroundRayInfo[] groundRayInfos;
    [SerializeField] private LayerMask groundLayer;
    public bool updateGrounded = true;

    // [Header("Edge")]
    // [SerializeField] private Vector3 edgeGroundOffset;
    // [SerializeField] private float groundCheckDist;
    // [SerializeField] private bool updateEdge = true;

    // [Header("Wall")]
    // [SerializeField] private Vector3 wallOffset;
    // [SerializeField] private float wallCheckDist;
    // [SerializeField] private bool useExcludeLayer = true;
    // [SerializeField] [ShowIf("useExcludeLayer")] LayerMask wallExcludeLayer;
    // [SerializeField] [HideIf("useExcludeLayer")] LayerMask wallLayer;
    // [SerializeField] private bool updateWall = true;

    [Header("DrawGizmo")]
    public bool showGizmo = true;

    private int movingPlatformTagHash;

    public Transform GroundTransform { get; private set; }
    // public bool isOnEdge { get; private set; }
    // public bool isTouchingWall { get; private set; }
    public bool isGrounded { get; private set; }

    private Transform owner => transform.parent;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Physics2D.queriesHitTriggers = false;
        if (updateGrounded) isGrounded = IsGrounded();
    }
    

    public bool IsOnEdge()
    {
        return false;
    }

    public bool IsTouchingWall()
    {
        return false;
    }

    public bool IsGrounded()
    {
        foreach (var _groundInfo in groundRayInfos)
        {
            var _pos = owner.position.Add(_groundInfo.groundOffset);
            if (!Physics.Raycast
                    (_pos, -owner.up, out var _hit, 
                        _groundInfo.groundDistance, groundLayer)) continue;
            
            GroundTransform = _hit.transform;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if(!showGizmo) return;
        Gizmos.color = Color.red;

        foreach (var _i in groundRayInfos)
        {
            var _pos = owner.position.Add(_i.groundOffset);
            Gizmos.DrawLine(_pos, _pos + (-owner.up * _i.groundDistance));
        }
    }
}
