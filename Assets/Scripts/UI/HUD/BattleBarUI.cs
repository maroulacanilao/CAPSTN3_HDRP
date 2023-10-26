using System;
using BattleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD
{
    [DefaultExecutionOrder(2)]
    public class BattleBarUI : HealthBarUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected BattleStation battleStation;
        [SerializeField] protected StatusEffectHUD statusEffectHUD;
        [SerializeField] protected TextMeshProUGUI name_TXT;
        [SerializeField] protected TextMeshProUGUI level_TXT;
        [SerializeField] protected GameObject selectIndicator;

        
        protected BattleCharacter battleCharacter;

        protected override void Start()
        {
            if (battleStation != null)
            {
                if (battleStation.battleCharacter == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                
                battleCharacter = battleStation.battleCharacter;
                character = battleCharacter.character;
                BattleCharacter.OnSelectMenu.AddListener(OnSelectMenuHandler);
            }
            
            base.Start();
            
            name_TXT.text = $"{character.characterData.characterName}";
            if (level_TXT != null) level_TXT.text = $"{character.level}";
            if (statusEffectHUD != null) statusEffectHUD.Initialize(character.statusEffectReceiver);
        }

        protected virtual void OnEnable()
        {
            if(battleCharacter != null) BattleCharacter.OnSelectMenu.AddListener(OnSelectMenuHandler);
        }

        protected virtual void OnDisable()
        {
            BattleCharacter.OnSelectMenu.RemoveListener(OnSelectMenuHandler);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            BattleCharacter.OnSelectMenu.RemoveListener(OnSelectMenuHandler);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            BattleCharacter.OnSelectMenu.Invoke(battleCharacter);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            BattleCharacter.OnSelectMenu.Invoke(null);
        }
        
        protected virtual void OnSelectMenuHandler(BattleCharacter battleCharacter_)
        {
            if(battleCharacter == null) return;
            if (selectIndicator != null) selectIndicator.SetActive(battleCharacter_ == battleCharacter);
        }
    }
}
