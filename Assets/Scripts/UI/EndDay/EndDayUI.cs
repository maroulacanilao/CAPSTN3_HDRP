using System;
using System.Collections;
using System.Threading.Tasks;
using CustomEvent;
using DG.Tweening;
using Managers;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndDay
{
    public class EndDayUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] TextMeshProUGUI dayCount_TXT;
        [SerializeField] private TextMeshProUGUI save_TXT;
        [SerializeField] float effectDuration = 0.5f;
        [SerializeField] Button startDay_BTN;
        [SerializeField] GameObject gameOverPanelPrefab;
        
        public static readonly Evt OnShowEndDayUI = new Evt();

        Vector3 originalScale;
        Vector3 targetScale;

        private void Awake()
        {
            originalScale = dayCount_TXT.transform.localScale;
            targetScale = originalScale * 1.5f;
            panel.SetActive(false);
            OnShowEndDayUI.AddListener(EndDay);
            startDay_BTN.onClick.AddListener(StartDayClick);
            save_TXT.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnShowEndDayUI.RemoveListener(EndDay);
            startDay_BTN.onClick.RemoveListener(StartDayClick);
        }

        private void EndDay()
        {
            StartCoroutine(Co_EndDay());
        }

        private void StartDayClick()
        {
            panel.SetActive(false);
            TimeManager.BeginDay();
            TimeManager.PauseTime();
            TimeManager.ResumeTime();
        }

        private IEnumerator Co_EndDay()
        {
            PlayerMenuManager.OnCloseAllUI.Invoke();
            startDay_BTN.gameObject.SetActive(false);

            Time.timeScale = 0;
            panel.SetActive(true);
            dayCount_TXT.text = $"Day {TimeManager.DayCounter}";
            dayCount_TXT.transform.localScale = originalScale;
            
            yield return new WaitForSecondsRealtime(0.1f);

            if (TimeManager.DaysLeft <= 0)
            {
                Time.timeScale = 0;
                Debug.Log("<color=red> Day counter is 0 or less. </color>");
                Instantiate(gameOverPanelPrefab, transform.parent);
                yield break;
            }

            StartCoroutine(Co_Save());


            yield return new WaitForSecondsRealtime(1f);

            yield return dayCount_TXT.transform
                .DOScale(targetScale, effectDuration)
                .SetEase(Ease.InSine)
                .SetUpdate(true)
                .WaitForCompletion();
            
            dayCount_TXT.text = $"Day {TimeManager.DayCounter + 1}";
            
            yield return dayCount_TXT.transform
                .DOScale(originalScale, effectDuration)
                .SetEase(Ease.OutSine)
                .SetUpdate(true)
                .WaitForCompletion();

            yield return new WaitForSecondsRealtime(1f);
            
            startDay_BTN.gameObject.SetActive(true);
        }
        
        private IEnumerator Co_Save()
        {
            save_TXT.gameObject.SetActive(true);
            save_TXT.color = Color.white;
            try
            {
                GameManager.Save();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            yield return new WaitForSeconds(0.5f);
            save_TXT.DOFade(0f, 1f);
            save_TXT.gameObject.SetActive(false);
        }
    }
}
