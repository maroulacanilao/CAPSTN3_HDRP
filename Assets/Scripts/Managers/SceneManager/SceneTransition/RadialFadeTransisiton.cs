using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.SceneManager.SceneTransition
{
    public class RadialFadeTransisiton : FadeTransition_Base
    {
        private Image image;
    
        protected override void Setup()
        {
            image = imgRect.GetComponent<Image>();
        }
    
        protected override Task OnTransition()
        {
            float size = screenSize.x > screenSize.y ? screenSize.x : screenSize.y;
            size *= screenSize.x > screenSize.y ? (screenSize.x / screenSize.y) : (screenSize.y / screenSize.x);
        
            Vector2 circleSize = new Vector2(size * 2f, size * 2f );
        
            imgRect.sizeDelta = circleSize;

            int waitDur = Mathf.RoundToInt(duration  * 10);
            image.fillAmount = isStartScene ? 1f : 0;
            float endFill = isStartScene ? 0 : 1f;

            Tween tween = image.DOFillAmount(endFill, duration);
            tween.SetUpdate(true);

            return tween.AsyncWaitForCompletion();
        }
    }
}
