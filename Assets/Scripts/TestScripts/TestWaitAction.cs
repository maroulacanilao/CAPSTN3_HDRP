using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomEvent;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class TestWaitAction : MonoBehaviour
{
    public string paramName;

    private Dictionary<string, TaskCompletionSource<bool>> animationEventTaskCompletionSources;

    private void Awake()
    {
        
        animationEventTaskCompletionSources = new Dictionary<string, TaskCompletionSource<bool>>();
    }
}
