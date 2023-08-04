using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ObjectPool;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private Vector3 rotationOffset;
    public void OnSpawn()
    {
        
    }
    public void OnDeSpawn()
    {
        StopAllCoroutines();
    }

    public IEnumerator StartProjectile(Vector3 targetPos_, bool lookAtTarget_ = true, float duration_ = 0.5f, Ease ease_ = Ease.Linear)
    {
        if(lookAtTarget_) LookAtTarget(targetPos_);
        var _tween = this.transform.DOMove(targetPos_, duration_).SetEase(ease_);
        yield return AdditionalEffects(targetPos_, duration_, ease_);
        yield return _tween.WaitForCompletion();
    }

    protected virtual IEnumerator AdditionalEffects(Vector3 targetPos_, float duration_ = 0.5f, Ease ease_ = Ease.Linear)
    {
        yield break;
    }
    
    private void LookAtTarget(Vector3 targetPos_)
    {
        var _dir = targetPos_ - this.transform.position;
        var _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        this.transform.Rotate(rotationOffset);
    }
}
