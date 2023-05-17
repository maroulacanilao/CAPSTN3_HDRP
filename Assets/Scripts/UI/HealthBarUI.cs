using BaseCore;
using UnityEngine;
using UnityEngine.UI;
using Character;
using DG.Tweening;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private TextMeshProUGUI hpText, nameText;
    [SerializeField] private Image hpBar;
    [SerializeField] private float effectDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green;
    
    Color originalColor;
    
    private void Awake()
    {
        healthComponent.OnTakeDamage.AddListener(DamageEffect);
        healthComponent.OnHeal.AddListener(HealEffect);
        nameText.text = healthComponent.transform.parent.GetComponent<CharacterBase>().characterData.characterName;
        originalColor = hpBar.color;
    }

    private void Start()
    {
        hpText.text = $"Health: {healthComponent.CurrentHp}/{healthComponent.MaxHp}";
    }

    private void OnDestroy()
    {
        healthComponent.OnTakeDamage.RemoveListener(DamageEffect);
        healthComponent.OnHeal.RemoveListener(HealEffect);
    }

    private async void DamageEffect(HealthComponent hpComponent_, DamageInfo damageInfo_)
    {
        transform.DOShakePosition(0.25f, 10f);
        hpBar.color = damageColor;
        var _percentage = hpComponent_.HpPercentage;
        UpdateHpText();
        await hpBar.DOFillAmount(_percentage, effectDuration).AsyncWaitForCompletion();
        if(_percentage> 0.2f) hpBar.color = originalColor;
    }
    
    private async void HealEffect(HealthComponent hpComponent_, HealInfo healInfo_)
    {
        hpBar.color = healColor;
        UpdateHpText();
        await hpBar.DOFillAmount(hpComponent_.HpPercentage, effectDuration).AsyncWaitForCompletion();
        hpBar.color = originalColor;
    }
    
    private void UpdateHpText()
    {
        var _prevVal = Mathf.FloorToInt(healthComponent.MaxHp * hpBar.fillAmount);
        int _targetVal = healthComponent.CurrentHp;
        
        DOTween.To(() => _prevVal, x => _prevVal = x, _targetVal, effectDuration).OnUpdate(() =>
        {
            hpText.text = $"Health: {_prevVal}/{healthComponent.MaxHp}";
        });
    }
}
