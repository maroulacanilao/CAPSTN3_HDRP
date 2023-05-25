using CustomEvent;
using UnityEngine;

namespace BaseCore
{
    [System.Serializable]
    public class BlessingMeter
    {
        [SerializeField] private float maxBlessing = 100f;
        private float currentBlessing;

        public float CurrentBlessing => currentBlessing;
        public float MaxBlessing => maxBlessing;
        
        public static readonly Evt<BlessingMeter> OnBlessingChange = new Evt<BlessingMeter>();

        public BlessingMeter()
        {
            currentBlessing = maxBlessing / 2f;
        }
        
        public void AddBlessing(float blessingAmount_)
        {
            currentBlessing += blessingAmount_;
            ClampBlessing();
        }
        
        public void RemoveBlessing(float blessingAmount_)
        {
            currentBlessing -= blessingAmount_;
            ClampBlessing();
        }

        private void ClampBlessing()
        {
            currentBlessing = Mathf.Clamp(currentBlessing, 0, maxBlessing);
            OnBlessingChange.Invoke(this);
        }
    }
}
