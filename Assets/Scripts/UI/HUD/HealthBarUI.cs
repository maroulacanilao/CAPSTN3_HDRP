using BaseCore;
using Character;
using Character.CharacterComponents;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] protected CharacterBase character;
        [SerializeField] protected TextMeshProUGUI hpText;
        [SerializeField] protected Image hpBar;
        [SerializeField] protected float effectDuration = 0.1f;
        [SerializeField] protected Color damageColor = Color.red;
        [SerializeField] protected Color healColor = Color.green;
        
        Color originalColor;
        protected CharacterHealth characterHealth;

        protected virtual void Start()
        {
            characterHealth = character.health;
            characterHealth.OnTakeDamage.AddListener(DamageEffect);
            characterHealth.OnHeal.AddListener(HealEffect);
            originalColor = hpBar.color;
            hpText.text = $"Health: {characterHealth.CurrentHp}/{characterHealth.MaxHp}";
            hpBar.fillAmount = characterHealth.HpPercentage;
        }

        private void OnDestroy()
        {
            if(characterHealth == null) return;
            characterHealth.OnTakeDamage.RemoveListener(DamageEffect);
            characterHealth.OnHeal.RemoveListener(HealEffect);
        }

        private async void DamageEffect(CharacterHealth hp_, DamageInfo damageInfo_)
        {
            transform.DOShakePosition(0.25f, 10f);
            hpBar.color = damageColor;
            var _percentage = hp_.HpPercentage;
            UpdateHpText();
            await hpBar.DOFillAmount(_percentage, effectDuration).SetUpdate(true).AsyncWaitForCompletion();
            if(_percentage> 0.2f) hpBar.color = originalColor;
        }
    
        private async void HealEffect(CharacterHealth hp_, HealInfo healInfo_)
        {
            hpBar.color = healColor;
            UpdateHpText();
            await hpBar.DOFillAmount(hp_.HpPercentage, effectDuration).SetUpdate(true).AsyncWaitForCompletion();
            hpBar.color = originalColor;
        }
    
        private void UpdateHpText()
        {
            var _prevVal = Mathf.FloorToInt(characterHealth.MaxHp * hpBar.fillAmount);
            int _targetVal = characterHealth.CurrentHp;
        
            DOTween.To(() => _prevVal, x => _prevVal = x, _targetVal, effectDuration).SetUpdate(true).OnUpdate(() =>
            {
                hpText.text = $"Health: {_prevVal}/{characterHealth.MaxHp}";
            });
        }
    }
}
