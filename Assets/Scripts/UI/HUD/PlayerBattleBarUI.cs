using System.Collections;
using Character;
using Character.CharacterComponents;
using DG.Tweening;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerBattleBarUI : BattleBarUI
    {
        [SerializeField] private Image manaBar;
        [SerializeField] private TextMeshProUGUI mana_TXT;

        private CharacterMana characterMana;
        private PlayerData playerData;
        
        protected override void Start()
        {
            base.Start();
            characterMana = character.mana;
            playerData = character.characterData as PlayerData;
            characterMana.OnUseMana.AddListener(UpdateMana);
            characterMana.OnAddMana.AddListener(UpdateMana);
            characterHealth.OnManuallyUpdateHealth.AddListener(ManuallyUpdateHealthBar);
            characterMana.OnManuallyUpdateMana.AddListener(ManuallyUpdateManaBar);
            
            ManuallyUpdateHealthBar(characterHealth);
            ManuallyUpdateManaBar(characterMana);
        }
        
        private void OnDestroy()
        {
            characterHealth.OnManuallyUpdateHealth.RemoveListener(ManuallyUpdateHealthBar);
            characterMana.OnManuallyUpdateMana.RemoveListener(ManuallyUpdateManaBar);
            characterMana.OnUseMana.RemoveListener(UpdateMana);
            characterMana.OnAddMana.RemoveListener(UpdateMana);
        }

        private void UpdateMana(CharacterMana characterMana_)
        {
            UpdateManaText();
            manaBar.DOFillAmount(characterMana.ManaPercentage, effectDuration).SetUpdate(true);
        }
        
        private void UpdateManaText()
        {
            var _prevVal = Mathf.FloorToInt(characterMana.MaxMana * manaBar.fillAmount);
            int _targetVal = characterMana.CurrentMana;
        
            DOTween.To(() => _prevVal, x => _prevVal = x, _targetVal, effectDuration).SetUpdate(true).OnUpdate(() =>
            {
                mana_TXT.text = $"Mana: {_prevVal}/{characterMana.MaxMana}";
            });
        }

        private void ManuallyUpdateHealthBar(CharacterHealth characterHealth_)
        {
            hpBar.fillAmount = (float) playerData.CurrentHp / characterHealth.MaxHp;
            hpText.text = $"Health: {playerData.CurrentHp}/{characterHealth.MaxHp}";
        }
        
        private void ManuallyUpdateManaBar(CharacterMana characterMana_)
        {
            manaBar.fillAmount = (float) playerData.CurrentMana / characterMana.MaxMana;
            mana_TXT.text = $"Mana: {playerData.CurrentMana}/{characterMana.MaxMana}";
        }
    }
}
