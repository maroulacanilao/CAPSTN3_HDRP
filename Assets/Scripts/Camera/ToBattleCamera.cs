using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using CustomHelpers;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class ToBattleCamera : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart path;
    [SerializeField] private CinemachineVirtualCamera dollyCam;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Quaternion targetRotation = Quaternion.identity;
    
    [Button("Start Effect")]
    private void StartEffect()
    {
        StartCoroutine(Co_StartEffect(transform.position));
    }

    private void OnEnable()
    {
        dollyCam.Priority = 100;
        dollyCam.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        dollyCam.Priority = 1;
        dollyCam.gameObject.SetActive(false);
    }


    public IEnumerator Co_StartEffect(Vector3 position_, Transform lookAt_ = null)
    {
        transform.position = position_;
        
        path.m_Position = 0;
        dollyCam.Priority = 100;
        dollyCam.gameObject.SetActive(true);
        dollyCam.LookAt = lookAt_;

        var _update = new System.Action<float>(x => path.m_Position = x);
        
        yield return path.m_Position.DoFloat(endValue, duration, true, _update);
        
        yield return null;
    }
    
    public Task StartEffectAsync(Vector3 position_, Transform lookAt_ = null)
    {
        transform.SetPositionAndRotation(position_,targetRotation);
        
        path.m_Position = 0;
        dollyCam.Priority = 100;
        dollyCam.gameObject.SetActive(true);
        dollyCam.LookAt = lookAt_;
        
        var _update = new System.Action<float>(x => path.m_Position = x);
        var _onComplete = new System.Action(() =>
        {
            dollyCam.Priority = 0;
            dollyCam.gameObject.SetActive(false);
        });
        
        return path.m_Position.DoFloat(endValue, duration, true, _update, _onComplete);
    }
}
