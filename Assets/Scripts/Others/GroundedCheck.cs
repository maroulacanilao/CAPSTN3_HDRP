using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool drawSphere = true;

    private bool isGrounded;

    public bool IsGrounded => isGrounded;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, radius, groundLayer);

    }

    private void OnDrawGizmos()
    {
        if (!drawSphere) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public Collider GetGroundCollider()
    {
        if (!isGrounded) return null;

        var ray = new Ray(transform.position, Vector3.down);

        return Physics.Raycast(ray, out var hitInfo, radius, groundLayer) ?
            hitInfo.collider : null;
    }

    public Collider[] GetGroundColliders()
    {
        return Physics.OverlapSphere(transform.position, radius, groundLayer);
    }
}