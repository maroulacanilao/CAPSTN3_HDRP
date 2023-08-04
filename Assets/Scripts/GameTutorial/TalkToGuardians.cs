using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UI.HUD;
using UnityEngine;

public class TalkToGuardians : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<string, bool> KeyBoolPair = new SerializedDictionary<string, bool>();
    
    public void SetBool(string boolName_)
    {
        KeyBoolPair[boolName_] = true;
        OnGuardiansTalked();
    }
    
    public void OnGuardiansTalked()
    {
        var _maxCount = KeyBoolPair.Count;
        
        var _count = KeyBoolPair.Values.Count(b => b == true);

        if (_count >= _maxCount)
        {
            Fungus.Flowchart.BroadcastFungusMessage("AllGuardiansTalked");
            return;
        }
        
        ObjectiveHUD.OnUpdateObjectiveText.Invoke($"Talk to all guardians ({_count}/{_maxCount})");
    }
}
