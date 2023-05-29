using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Time_TXT;

        private void Awake()
        {
            TimeManager.OnMinuteTick.AddListener(UpdateText);
            UpdateText();
        }
    
        private void OnDestroy()
        {
            TimeManager.OnMinuteTick.RemoveListener(UpdateText);
        }

        private void UpdateText()
        {
            Time_TXT.SetText(TimeManager.DateTime.ToString("hh:mm tt"));
        }
    }
}