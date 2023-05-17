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
            UpdateText(TimeManager.Instance);
        }
    
        private void OnDestroy()
        {
            TimeManager.OnMinuteTick.RemoveListener(UpdateText);
        }

        private void UpdateText(TimeManager timeManager_)
        {
            Time_TXT.SetText(TimeManager.DateTime.ToString("hh:mm tt"));
        }
    }
}