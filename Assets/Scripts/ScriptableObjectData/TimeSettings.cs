using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "TimeSettings", menuName = "ScriptableObjects/TimeSettings", order = 0)]
    public class TimeSettings : ScriptableObject
    {
        [field: Header("Time Values: 24 hour format")]
        [field: BoxGroup("Ending Hour")] 
        [field: SerializeField] public bool isEndingHourNextDay { get; private set; } = true;
        [field: BoxGroup("Ending Hour")]
        [field: SerializeField] public int endingHour { get; private set; } = 24;
        
        [field: BoxGroup("Starting Hour")]
        [field: SerializeField] public int startingHour { get; private set; } = 6;
        [field: BoxGroup("Night Hour")]
        [field: SerializeField] public int nightHour { get; private set; } = 19;
        
        [field: BoxGroup("Tick Settings")]
        [field: SerializeField] public int minutePerTick { get; private set; } = 5;
        [field: BoxGroup("Tick Settings")]
        [field: SerializeField] public float tickRate { get; private set; } = 4.8f;
        
        [field: BoxGroup("Max Days")]
        [field: SerializeField] public int maxDays { get; private set; } = 30;
    }
}