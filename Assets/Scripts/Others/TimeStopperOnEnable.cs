using CustomHelpers;
using Managers;
using UnityEngine;

public class TimeStopperOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        TimeManager.Instance.PauseTime();
    }

    private void OnDisable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        TimeManager.Instance.ResumeTime();
    }
}
