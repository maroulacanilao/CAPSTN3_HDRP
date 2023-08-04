using System;
using System.Collections;
using CustomHelpers;
using DG.Tweening;
using NaughtyAttributes;
using Others.VFX;
using UnityEngine;

public class WoodenStakeVFX : EffectsPlayer
{
    [SerializeField] private Transform model;
    
    Vector3 originalPosition;
    
    private void Awake()
    {
        originalPosition = model.position;
    }

    public override void Play()
    {
        
    }
    
    public override IEnumerator Co_Play(float duration_ = 0)
    {
        transform.localPosition = Vector3.zero;
        var _targetPos = model.localPosition.AddX(-5); 
        yield return transform.DOLocalMove(_targetPos, 0.5f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
        yield return new WaitForSeconds(.1f);
        yield return transform.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
        StartCoroutine(Co_Release());
    }
    
    [Button("Try")] private void Try()
    {
        StartCoroutine(Co_Play());
    }

    public IEnumerator Co_Release()
    {
        yield return new WaitForSeconds(.05f);
        poolableObject.Release();
    }
}
