using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CustomHelpers
{
    public static class DOTweenHelper
    {
        public static IEnumerator Co_DoHitEffect(this Transform transform_, float duration_ = 0.75f, float strength_ = 0.5f)
        {
            yield return transform_.DOShakePosition(duration_, strength_).WaitForCompletion();
        }
        
        public static void DoHitEffect(this Transform transform_, float duration_ = 0.75f, float strength_ = 0.5f)
        {
            transform_.DOShakePosition(duration_, strength_);
        }
        
        
        
        public static IEnumerator DoBlink(this SpriteRenderer spriteRenderer_, float duration_ = 1, int loopsNum_ = 10)
        {
            // should always be even
            if (loopsNum_ % 2 == 1) loopsNum_++;
            var _dur = duration_ / loopsNum_ / 2;
            yield return spriteRenderer_.DOFade(0, _dur).SetLoops(loopsNum_, LoopType.Yoyo).WaitForCompletion();
        }
    }
}
