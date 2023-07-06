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
        yield return AdditionalEffects(targetPos_, duration_, ease_);
        yield return _tween.WaitForCompletion();
    }

    protected virtual IEnumerator AdditionalEffects(Vector3 targetPos_, float duration_ = 0.5f, Ease ease_ = Ease.Linear)
    {
        yield break;
    }
}
