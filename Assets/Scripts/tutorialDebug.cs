using System.Collections;
using System.Collections.Generic;
using Fungus;
using NaughtyAttributes;
using UnityEngine;

public class tutorialDebug : MonoBehaviour
{
    public Flowchart flowchart;
    public List<string> _list = new List<string>();
    public string message = "";
    
    [Button("end Tutorial")]
    private void EndTutorial()
    {
        foreach (var variable in _list)
        {
            flowchart.SetBooleanVariable(variable, true);
        }
        Fungus.Flowchart.BroadcastFungusMessage(message);
    }
}
