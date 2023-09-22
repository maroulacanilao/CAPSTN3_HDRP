using System.Collections;
using Character;
using Character.CharacterComponents;
using DG.Tweening;
using Fungus;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    [DefaultExecutionOrder(2)]
    public class PlayerBattleBarUI : BattleBarUI
    {
        [SerializeField] private Image manaBar;
        [SerializeField] private TextMeshProUGUI mana_TXT;

        private CharacterMana characterMana;
        
        private AllyData allyData;
        private PlayerData playerData;

        [SerializeField] bool isForAlly;
        
        protected override void Start()
        {
            if (character == null && !isForAlly)
            {
                character = FindObjectOfType<PlayerCharacter>();
                
                //Print debug log of what type is character right now
                //Debug.Log(character.GetType());
            }
            else if (character == null && isForAlly)
            {
                character = FindObjectOfType<AllyCharacter>();
                
                //Debug.Log(character.GetType());
            }
            
            base.Start();
            characterMana = character.mana;
            
            //If current character is ally then use ally data
            if(character is AllyCharacter) allyData = character.characterData as AllyData; 
            else playerData = character.characterData as PlayerData;
            
            characterMana.OnUseMana.AddListener(UpdateMana);
            characterMana.OnAddMana.AddListener(UpdateMana);
            characterHealth.OnManuallyUpdateHealth.AddListener(ManuallyUpdateHealthBar);
            characterMana.OnManuallyUpdateMana.AddListener(ManuallyUpdateManaBar);
            
            ManuallyUpdateHealthBar(characterHealth);
            ManuallyUpdateManaBar(characterMana);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            characterHealth?.OnManuallyUpdateHealth.RemoveListener(ManuallyUpdateHealthBar);
            characterMana?.OnManuallyUpdateMana.RemoveListener(ManuallyUpdateManaBar);
            characterMana?.OnUseMana.RemoveListener(UpdateMana);
            characterMana?.OnAddMana.RemoveListener(UpdateMana);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ManuallyUpdateManaBar(characterMana);
            ManuallyUpdateHealthBar(characterHealth);
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
            if(characterHealth == null) return;
            if(hpBar == null) return;
            if (character is AllyCharacter)
            {
                hpBar.fillAmount = (float) allyData.health.CurrentHp / allyData.health.MaxHp;
                hpText.text = $"Health: {allyData.health.CurrentHp}/{allyData.health.MaxHp}";
            }
            else
            {
                hpBar.fillAmount = (float)playerData.health.CurrentHp / playerData.health.MaxHp;
                hpText.text = $"Health: {playerData.health.CurrentHp}/{playerData.health.MaxHp}";
            }
            hpBar.color = hpBar.fillAmount > 0.2f ? originalColor : damageColor;
        }
        
        private void ManuallyUpdateManaBar(CharacterMana characterMana_)
        {
            if(characterMana == null) return;
            if(manaBar == null) return;
            if (character is AllyCharacter)
            {
                manaBar.fillAmount = (float) allyData.mana.CurrentMana / allyData.mana.MaxMana;
                mana_TXT.text = $"Mana: {allyData.mana.CurrentMana}/{allyData.mana.MaxMana}";
            }
            else
            {
                manaBar.fillAmount = (float) playerData.mana.CurrentMana / playerData.mana.MaxMana;
                mana_TXT.text = $"Mana: {playerData.mana.CurrentMana}/{playerData.mana.MaxMana}"; 
            }
            
        }
    }
}
