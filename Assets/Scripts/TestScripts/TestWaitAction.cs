using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class TestWaitAction : MonoBehaviour
{
    public int param1 = 2;
    private readonly Evt sampleEvent = new Evt();
    private readonly Evt<int> sampleEventWithParam = new Evt<int>();

    private IEnumerator Start()
    {
        Debug.Log("Start waiting for event");
        yield return sampleEvent.WaitForEvt();
        Debug.Log("Event called");
    }
    
    [Button("Call Event")]
    private void CallEvent() => sampleEvent.Invoke();
    
    [Button("Call Event With Param")]
    private void CallEventWithParam() => sampleEventWithParam.Invoke(param1);
}
