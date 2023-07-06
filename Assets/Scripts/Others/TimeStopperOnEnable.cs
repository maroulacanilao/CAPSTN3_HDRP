using CustomHelpers;
using Managers;
using UnityEngine;

public class TimeStopperOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        TimeManager.PauseTime();
    }

    private void OnDisable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        TimeManager.ResumeTime();
    }
}
