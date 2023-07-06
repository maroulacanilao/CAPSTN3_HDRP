using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CustomHelpers;
using Managers;
using NaughtyAttributes;
using Player;
using UnityEngine;

[DefaultExecutionOrder(99)]
public class CameraPlayerSetter : MonoBehaviour
{
    [SerializeField] [Tag] private string targetTag = "CameraTarget";
    private ICinemachineCamera cineCam;
    private string sceneName;
    
    private void Awake()
    {
        cineCam = GetComponent<ICinemachineCamera>();
        sceneName = gameObject.scene.name;
        if(CameraManager.IsInstanceValid()) CameraManager.Instance.RegisterPlayerCamera(sceneName, cineCam);
    }

    private void OnDestroy()
    {
        if(CameraManager.IsInstanceValid()) CameraManager.Instance.UnRegisterPlayerCamera(sceneName);
    }

    private void OnEnable()
    {
        var _player = PlayerInputController.Instance.transform;
        var _target = GameObject.FindWithTag(targetTag);
        
        cineCam.Follow = _player;
        
        cineCam.LookAt = _target != null ? _target.transform : _player;

        CinemachineBrain brain = gameObject.scene.FindFirstComponentInScene<CinemachineBrain>(true);
    }
}
