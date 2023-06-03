using System;
using BattleSystem;
using CustomEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Battle
{
    public class TargetButton : SelectableMenuButton, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI name_TXT;
        private BattleCharacter battleCharacter;
        private BattleActionUI battleActionUI;

        private static readonly Evt<TargetButton> OnTargetSelectButton = new Evt<TargetButton>();
        
        public TargetButton Initialize(BattleActionUI battleActionUI_,BattleCharacter character_)
        {
            battleCharacter = character_;
            
            if (battleCharacter == null)
            {
                Destroy(gameObject);
                return null;
            }
            
            battleActionUI = battleActionUI_;
            
            name_TXT.text = battleCharacter.characterData.characterName;
            button.onClick.AddListener(() => battleActionUI.StartAction(battleCharacter));
            OnTargetSelectButton.AddListener(OnSelectButtonHandler);
            return this;
        }

        private void OnDestroy()
        {
            OnTargetSelectButton.RemoveListener(OnSelectButtonHandler);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            button.interactable = battleCharacter.character.IsAlive;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            BattleCharacter.OnSelectMenu.Invoke(null);
        }

        public override void SelectButton()
        {
            base.SelectButton();
            if(!button.interactable) return;
            OnTargetSelectButton.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
        }
        
        private void OnSelectButtonHandler(TargetButton targetButton_)
        {
            if(targetButton_ != this) return;
            BattleCharacter.OnSelectMenu.Invoke(battleCharacter);
        }
    }
}
