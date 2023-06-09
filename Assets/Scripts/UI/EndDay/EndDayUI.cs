using System;
using System.Threading.Tasks;
using DG.Tweening;
using Managers;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndDay
{
    public class EndDayUI : PlayerMenu
    {

        [SerializeField] TextMeshProUGUI dayCount_TXT;
        [SerializeField] float effectDuration = 0.5f;
        [SerializeField] Button startDay_BTN;

        Vector3 originalScale;
        Vector3 targetScale;

        public override void Initialize()
        {
            TimeManager.OnEndDay.AddListener(EndDay);
            originalScale = dayCount_TXT.transform.localScale;
            targetScale = originalScale * 1.5f;
            startDay_BTN.onClick.AddListener(StartDayClick);
        }

        private async void EndDay()
        {
            PlayerMenuManager.OnCloseAllUI.Invoke();
            startDay_BTN.gameObject.SetActive(false);

            Time.timeScale = 0;
            gameObject.SetActive(true);
            dayCount_TXT.text = $"Day {TimeManager.DayCounter}";
            dayCount_TXT.transform.localScale = originalScale;
            
            await Task.Delay(1000);

            await dayCount_TXT.transform
                .DOScale(targetScale, effectDuration)
                .SetEase(Ease.InSine)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
            
            dayCount_TXT.text = $"Day {TimeManager.DayCounter + 1}";
            
            await dayCount_TXT.transform
                .DOScale(originalScale, effectDuration)
                .SetEase(Ease.OutSine)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
            
            await Task.Delay(500);
            
            startDay_BTN.gameObject.SetActive(true);
        }

        private void StartDayClick()
        {
            gameObject.SetActive(false);
            TimeManager.BeginDay();
            TimeManager.PauseTime();
            TimeManager.ResumeTime();
        }
    }
}
