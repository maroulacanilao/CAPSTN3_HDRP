using System;
using BattleSystem;
using CustomEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Battle
{
    public class TargetButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI name_TXT;
        private BattleCharacter battleCharacter;
        private BattleActionUI battleActionUI;

        private static readonly Evt<TargetButton> OnSelectButton = new Evt<TargetButton>();
        
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
            OnSelectButton.AddListener(OnSelectButtonHandler);
            return this;
        }

        private void OnDestroy()
        {
            OnSelectButton.RemoveListener(OnSelectButtonHandler);
        }

        private void OnEnable()
        {
            button.interactable = battleCharacter.character.IsAlive;
        }

        private void OnDisable()
        {
            BattleCharacter.OnSelectMenu.Invoke(null);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if(!button.interactable) return;
            OnSelectButton.Invoke(this);
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
