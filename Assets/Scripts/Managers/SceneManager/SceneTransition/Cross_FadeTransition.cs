using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.SceneLoader.SceneTransition
{
    public class Cross_FadeTransition : FadeTransition_Base
    {
        private Image image;

        protected override void Setup()
        {
            image = imgRect.GetComponent<Image>();
            image.raycastTarget = false;
        }
    
        protected override Task OnTransition()
        {
            image.color = isStartScene ? Color.black : Color.clear;
            Tween tween = image.DOColor(isStartScene ? Color.clear : Color.black, duration);
            tween.SetUpdate(true);

            return tween.AsyncWaitForCompletion();
        }
    }
}
