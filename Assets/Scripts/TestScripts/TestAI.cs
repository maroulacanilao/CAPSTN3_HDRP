using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    // [SerializeField]
    //
    // private Transform target;
    //
    // private NavMeshPath path;
    // private NativeArray<Vector3> followPosition;
    // private void Start()
    // {
    //     path = new NavMeshPath();
    //     followPosition = new NativeArray<Vector3>(1, Allocator.Persistent);
    //     
    //     var _job = new SampleJob(transform.position, target.position, 1f, Time.deltaTime, followPosition);
    //     
    //     var _jobHandle = _job.Schedule();
    //     _jobHandle.Complete();
    // }
    //
    // private void OnDisable()
    // {
    //     followPosition.Dispose();
    // }

    [Button("TRY")]
    void GetNavMeshPath()
    {
        // NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, navPath);
        //
        // foreach (var _corner in navPath.corners)
        // {
        //     Debug.Log(_corner);
        // }
    }
}

[BurstCompile]
public struct SampleJob : IJob
{
    public Vector3 currentPosition;
    public Vector3 targetPosition;
    public float LerpSpeed;
    public float deltaTime;
    public NativeArray<Vector3> resultPosition;

    public SampleJob(Vector3 currentPosition_, Vector3 targetPosition_, float lerpSpeed_, float deltaTime_, NativeArray<Vector3> resultPosition_)
    {
        currentPosition = currentPosition_;
        targetPosition = targetPosition_;
        LerpSpeed = lerpSpeed_;
        deltaTime = deltaTime_;
        resultPosition = resultPosition_;
    }
    
    public void Execute()
    {
        resultPosition[0] = Vector3.Lerp(currentPosition, targetPosition, LerpSpeed * deltaTime);
    }
}
