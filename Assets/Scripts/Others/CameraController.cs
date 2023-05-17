using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float zOffset = -10f;
    [SerializeField] private float yOffset = 10f;

    private Vector3 playerPos => player.position;

    private void Awake()
    {
        if (!player) player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = playerPos.Add(0, yOffset, zOffset);
    }
}
