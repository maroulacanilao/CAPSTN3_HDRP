using System;
using System.Collections;
using System.Collections.Generic;
using BaseCore;
using ScriptableObjectData.CharacterData;
using UnityEngine;

public class BorderTeleport : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform topBorderTransform, bottomBorderTransform, leftBorderTransform, rightBorderTransform, frontBorderTransform, backBorderTransform;
    [SerializeField] private Transform teleportTransform;
    [SerializeField] private int fallDamage = 10;

    private Vector3 leftBorder;
    private Vector3 rightBorder;
    private Vector3 topBorder;
    private Vector3 bottomBorder;
    private Vector3 frontBorder;
    private Vector3 backBorder;
    private Vector3 teleportPosition;
    
    private Transform playerTransform;
    private Vector3 playerPosition => playerTransform.position;

    private void Start()
    {
        leftBorder = leftBorderTransform.position;
        rightBorder = rightBorderTransform.position;
        topBorder = topBorderTransform.position;
        bottomBorder = bottomBorderTransform.position;
        frontBorder = frontBorderTransform.position;
        backBorder = backBorderTransform.position;
        teleportPosition = teleportTransform.position;
    }

    private void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if(playerPosition.x < leftBorder.x) Debug.Log("Left");//Teleport();
        else if(playerPosition.x > rightBorder.x) Debug.Log("Right");//Teleport();
        else if (playerPosition.y > topBorder.y) return;//Teleport();
        else if(playerPosition.y < bottomBorder.y) return;//Teleport();
        else if(playerPosition.z < frontBorder.z) Debug.Log("Front");//Teleport();
        else if(playerPosition.z > backBorder.z) Debug.Log("Back");//Teleport();
    }

    private void Teleport()
    {
        playerTransform.gameObject.SetActive(false);
        var _dam = new DamageInfo(fallDamage, null);
        playerData.health.TakeDamage(_dam);
        playerTransform.position = teleportPosition;
        playerTransform.gameObject.SetActive(true);
    }
}
