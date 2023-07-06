using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class EffectIconItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        
        private StatusEffectBase statusEffect;
        
        private void Reset()
        {
            image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        private void Awake()
        {
            if(image == null) image = GetComponent<Image>();
            if(text == null) text = GetComponentInChildren<TextMeshProUGUI>();
        }
    
        public void SetIcon(StatusEffectBase statusEffect_)
        {
            statusEffect = statusEffect_;
            image.sprite = statusEffect.Icon;
            
            text.text = statusEffect.HasDuration ? statusEffect.turnsLeft.ToString() : string.Empty;
            
            statusEffect.OnTurnsLeftChange.AddListener(UpdateText);
        }
        
        private void UpdateText(int turnLeft_)
        {
            text.text = statusEffect.HasDuration ? statusEffect.turnsLeft.ToString() : string.Empty;
        }
    }
}
