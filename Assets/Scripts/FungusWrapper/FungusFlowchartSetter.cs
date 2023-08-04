using System;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace FungusWrapper
{
    [DefaultExecutionOrder(-1)]
    public class FungusFlowchartSetter : MonoBehaviour
    {
        [SerializeField] private Flowchart flowchart;
        
        public static List<Flowchart> flowchartList = new List<Flowchart>();


        private void Reset()
        {
            flowchart = GetComponent<Flowchart>();
        }
        
        private void Awake()
        {
            if(flowchart == null)flowchart = GetComponent<Flowchart>();
            flowchartList.Add(flowchart);
            FungusManager.Instance.useUnscaledTime = true;
        }
        
        private void OnDestroy()
        {
            flowchartList.Remove(flowchart);
        }

        public static bool IsExecuting()
        {
            Debug.Log($"flowchart list count: {flowchartList.Count}");
            PurgeNulls();
            if(flowchartList.Count == 0) return false;
            

            
            foreach (var _flow in flowchartList)
            {
                if(_flow == null) continue;
                Debug.Log($"flowchart: {_flow.name} is active: {_flow.isActiveAndEnabled} has executing blocks: {_flow.HasExecutingBlocks()}");
                if(!_flow.isActiveAndEnabled) continue;
                if(_flow.HasExecutingBlocks()) return true;
            }
            
            return false;
        }
        
        public static void StopAll()
        {
            foreach (var VARIABLE in flowchartList)
            {
                VARIABLE.StopAllBlocks();
            }
        }

        private static void PurgeNulls()
        {
            flowchartList.RemoveAll(item => item == null);
        }
    }
}