using System.Threading.Tasks;
using CustomHelpers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.SceneManager.SceneTransition
{
    public class DualBoxFillFadeTransisiton : FadeTransition_Base
    {
        [SerializeField] private RectTransform rect_2;

        private Image image_1;
        private Image image_2;
    
        protected override void Setup()
        {
            image_1 = imgRect.GetComponent<Image>();
            image_2 = rect_2.GetComponent<Image>();
            var sizeX = Screen.width / 2f;
        
            imgRect.sizeDelta = imgRect.sizeDelta.SetX(sizeX);
            imgRect.anchoredPosition = imgRect.anchoredPosition.SetX(-sizeX / 2f);

            rect_2.sizeDelta = rect_2.sizeDelta.SetX(sizeX);
            rect_2.anchoredPosition = rect_2.anchoredPosition.SetX(sizeX / 2f);
        }

        protected override Task OnTransition()
        {
            image_1.fillOrigin = isStartScene ? 
                (int)Image.OriginVertical.Bottom : 
                (int)Image.OriginVertical.Top;
        
            image_2.fillOrigin = isStartScene ? 
                (int)Image.OriginVertical.Top : 
                (int)Image.OriginVertical.Bottom;

        
            image_1.fillAmount = isStartScene ? 1f : 0;
            image_2.fillAmount = isStartScene ? 1f : 0;
        
            float endFill = isStartScene ? 0 : 1f;


            Tween tween1 = image_1.DOFillAmount(endFill, duration);
            Tween tween2 = image_2.DOFillAmount(endFill, duration);
            tween1.SetUpdate(true);
            tween2.SetUpdate(true);

            return tween1.AsyncWaitForCompletion();
        }
    }
}
