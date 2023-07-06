using CustomHelpers;
using Managers;
using NaughtyAttributes;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    [SerializeField] private bool timeScaleOnly = false;
    [SerializeField] [HideIf("timeScaleOnly")] private bool pauseTimeManagerScale = true;
    
    private void OnEnable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        if (timeScaleOnly) Time.timeScale = 0;
        else TimeManager.PauseTime(pauseTimeManagerScale);
    }

    private void OnDisable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        if (timeScaleOnly) Time.timeScale = 1;
        else TimeManager.ResumeTime();
    }
}
