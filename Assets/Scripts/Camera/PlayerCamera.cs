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
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] [Tag] private string targetTag = "CameraTarget";
    public CinemachineVirtualCamera cineCam {get ; private set;}
    public string sceneName {get ; private set;}

    public float defaultFOV;

    private void Awake()
    {
        cineCam = GetComponent<CinemachineVirtualCamera>();
        
        sceneName = gameObject.scene.name;
        
        CameraManager.RegisterPlayerCamera(sceneName, this);
        
        FollowAndLookAtPlayer();

        defaultFOV = cineCam.m_Lens.OrthographicSize;
    }

    private void OnDestroy()
    {
        CameraManager.UnRegisterPlayerCamera(sceneName);
    }

    private void OnEnable()
    {
        FollowAndLookAtPlayer();
    }

    private void FollowAndLookAtPlayer()
    {
        var _player = PlayerInputController.Instance.transform;
        var _target = GameObject.FindWithTag(targetTag);
        
        cineCam.Follow = _player;
        
        cineCam.LookAt = _target != null ? _target.transform : _player;
    }
    
    public IEnumerator Co_ZoomIn(float duration_, float targetFOV_)
    {
        var _startFOV = cineCam.m_Lens.OrthographicSize;
        var _timer = 0f;
        Debug.Log("Start Size: " + cineCam.m_Lens.OrthographicSize);
    
        while (_timer < duration_)
        {
            _timer += Time.unscaledDeltaTime;
            Debug.Log(_timer);
        
            float t = _timer / duration_;
            float currentFOV = Mathf.Lerp(_startFOV, _startFOV * 2, t);
            cineCam.m_Lens.OrthographicSize = currentFOV;
            Debug.Log(cineCam.m_Lens.OrthographicSize);
        
            yield return null;
        }
    
        Debug.Log("Finish Size: " + cineCam.m_Lens.OrthographicSize);
        Debug.Log(gameObject.scene.name);
    }
}
