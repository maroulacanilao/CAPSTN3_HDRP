using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "LightingData", menuName = "ScriptableObjects/LightingData", order = 0)]
    public class LightingData : ScriptableObject
    {
        [field: BoxGroup("Day")]
        [field: SerializeField] public int dayStartHour { get; private set;}
        [field: BoxGroup("Day")]
        [field: SerializeField] public Color dayLightColor { get; private set;}
        [field: BoxGroup("Day")]
        [field: SerializeField] public float dayTemperature { get; private set;}
        [field: BoxGroup("Day")]
        [field: SerializeField] public float dayIntensity { get; private set;}
        [field: BoxGroup("Day")] [field: MinMaxSlider(0,360)]
        [field: SerializeField] public Vector2 dayYRotationRange { get; private set;}
        
    
        [field: BoxGroup("Night")]
        [field: SerializeField] public int NightStartHour { get; private set;}
        [field: BoxGroup("Night")]
        [field: SerializeField] public Color nightLightColor { get; private set;}
        [field: BoxGroup("Night")]
        [field: SerializeField] public float nightTemperature { get; private set;}
        [field: BoxGroup("Night")]
        [field: SerializeField] public float nightIntensity { get; private set;}
        [field: BoxGroup("Night")] [field: MinMaxSlider(0,360)]
        [field: SerializeField] public Vector2 nightYRotationRange { get; private set;}
    
        [field: BoxGroup("Curves")] [field: CurveRange(0,0,2.4f,2.4f,EColor.Yellow)]
        [field: SerializeField] public AnimationCurve intensityCurve { get; private set;}
        [field: BoxGroup("Curves")] [field: CurveRange(0,0,2.4f,2.4f,EColor.Yellow)]
        [field: SerializeField] public AnimationCurve colorCurve { get; private set;}
        [field: BoxGroup("Curves")] [field: CurveRange(0,0,2.4f,2.4f,EColor.Yellow)]
        [field: SerializeField] public AnimationCurve temperatureCurve { get; private set;}
        [field: BoxGroup("Curves")] [field: CurveRange(0,0,2.4f,2.4f,EColor.Yellow)]
        [field: SerializeField] public AnimationCurve rotationCurve { get; private set;}
    }
}