using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ObjectPool;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    public void OnSpawn()
    {
        
    }
    public void OnDeSpawn()
    {
        StopAllCoroutines();
    }

    public IEnumerator StartProjectile(Vector3 targetPos_, float duration_ = 0.5f, Ease ease_ = Ease.Linear)
    {
        var _tween = this.transform.DOMove(targetPos_, duration_).SetEase(ease_);
        AdditionalEffects(duration_, ease_);
        yield return _tween.WaitForCompletion();
    }

    protected virtual void AdditionalEffects(float duration_ = 0.5f, Ease ease_ = Ease.Linear)
    {
        return;
    }
}
