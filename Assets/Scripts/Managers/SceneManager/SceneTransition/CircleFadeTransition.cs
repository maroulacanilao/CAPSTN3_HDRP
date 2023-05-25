using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Managers.SceneManager.SceneTransition
{
    public class CircleFadeTransition : FadeTransition_Base
    {
        private Vector2 bigSize;
        protected override void Setup()
        {
            base.Setup();
            float wide = Screen.width * 1.5f;
            bigSize = new Vector2(wide, wide);
        }
    
        protected override Task OnTransition()
        {
            imgRect.sizeDelta = isStartScene ? bigSize : Vector2.one;
        
            Vector2 endSize = isStartScene ? Vector2.one : bigSize;
        
            Tweener tween = imgRect.DOSizeDelta(endSize, duration);
            tween.SetUpdate(true);
        
            return tween.AsyncWaitForCompletion();
        }
    }
}
