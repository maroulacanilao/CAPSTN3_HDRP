using DG.Tweening;
using Managers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Time_TXT;
        [SerializeField] private TextMeshProUGUI daysLeft_Txt;

        [SerializeField] private Image dayCycle_Image;
        [SerializeField] private Sprite[] dayNightIcons;

        private Vector3 originalPos;

        private void Awake()
        {
            originalPos = dayCycle_Image.transform.localPosition;

            TimeManager.OnMinuteTick.AddListener(UpdateText);
            TimeManager.OnBeginDay.AddListener(UpdateDaysRemaining);
            TimeManager.OnBeginDay.AddListener(UpdateDayCycle);
            TimeManager.OnNightTime.AddListener(UpdateDayCycle);
            UpdateText();
            daysLeft_Txt.gameObject.SetActive(false);
        }
    
        private void OnDestroy()
        {
            TimeManager.OnMinuteTick.RemoveListener(UpdateText);
        }

        private void UpdateText()
        {
            Time_TXT.SetText(TimeManager.DateTime.ToString("hh:mm tt"));
        }

        private void UpdateDaysRemaining()
        {
            if (SceneManager.GetActiveScene().name != GameManager.Instance.TutorialSceneName)
            {
                daysLeft_Txt.text = $"{TimeManager.DaysLeft} days remaining";
                daysLeft_Txt.gameObject.SetActive(true);
            }
        }

        private void UpdateDayCycle()
        {
            if (TimeManager.IsNight())
            {
                StartCoroutine(PlayDayCycleAnimation(dayNightIcons[1]));
            }
            else
            {
                dayCycle_Image.sprite = dayNightIcons[0];
            }
        }

        IEnumerator PlayDayCycleAnimation(Sprite cycleIcon)
        {
            var exitSequence = DOTween.Sequence();
            exitSequence.Append(dayCycle_Image.transform.DOLocalMoveY(originalPos.y + 200f, 1f));
            exitSequence.Join(dayCycle_Image.DOFade(0f, .5f));
            dayCycle_Image.sprite = cycleIcon;

            yield return new WaitForSeconds(0.5f);

            var returnSequence = DOTween.Sequence();
            returnSequence.Append(dayCycle_Image.transform.DOLocalMoveY(originalPos.y, 1f));
            returnSequence.Join(dayCycle_Image.DOFade(1f, .5f));
        }
    }
}