using System;
using BattleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD
{
    public class BattleBarUI : HealthBarUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BattleStation battleStation;
        [SerializeField] private TextMeshProUGUI name_TXT;
        [SerializeField] private GameObject selectIndicator;
        
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
            }
            
            base.Start();
            
            name_TXT.text = character.characterData.characterName;
        }

        protected virtual void OnEnable()
        {
            if(battleCharacter != null) BattleCharacter.OnSelectMenu.AddListener(OnSelectMenuHandler);
        }

        protected virtual void OnDisable()
        {
            if(battleCharacter != null) BattleCharacter.OnSelectMenu.RemoveListener(OnSelectMenuHandler);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("enter UI on" + character.gameObject.name);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("exit UI on" + character.gameObject.name);
        }
        
        private void OnSelectMenuHandler(BattleCharacter battleCharacter_)
        {
            if(battleCharacter == null) return;
            selectIndicator.SetActive(battleCharacter_ == battleCharacter);
        }
    }
}
