using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

namespace Managers.SceneLoader.SceneTransition
{
    public class BoxFadeTransition : FadeTransition_Base
    {
        private Image image;
        protected override void Setup()
        {
            image = imgRect.GetComponent<Image>();
        }

        protected override Task OnTransition()
        {
            image.fillOrigin = isStartScene ? (int)Image.OriginHorizontal.Left : (int) Image.OriginHorizontal.Right;
        
            image.fillAmount = isStartScene ? 1f : 0;
            float endFill = isStartScene ? 0 : 1f;


            Tween tween = image.DOFillAmount(endFill, duration);
            tween.SetUpdate(true);
        
            return tween.AsyncWaitForCompletion();
        }
    }
}
