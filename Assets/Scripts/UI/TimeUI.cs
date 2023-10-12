using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Time_TXT;
        [SerializeField] private TextMeshProUGUI daysLeft_Txt;

        // [SerializeField] private Image[] dayCyclesUI;
        // [SerializeField] private Sprite[] dayOrNightIcon;

        private void Awake()
        {
            TimeManager.OnMinuteTick.AddListener(UpdateText);
            TimeManager.OnBeginDay.AddListener(UpdateDaysRemaining);
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
            daysLeft_Txt.text = $"{TimeManager.DaysLeft} days remaining";
            daysLeft_Txt.gameObject.SetActive(true);
        }

        private void UpdateDayCycle()
        {

        }
    }
}