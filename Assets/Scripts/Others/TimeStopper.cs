using CustomHelpers;
using Managers;
using NaughtyAttributes;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    [SerializeField] protected bool timeScaleOnly = false;
    [SerializeField] [HideIf("timeScaleOnly")] protected bool pauseTimeManagerScale = true;
    
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
