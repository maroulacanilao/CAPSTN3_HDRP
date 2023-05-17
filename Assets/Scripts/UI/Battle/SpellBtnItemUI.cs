using System;
using CustomEvent;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Battle
{
    public class SpellBtnItemUI : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonTXT;
        private BattleActionUI mainPanel;
        public int spellIndex { get; private set; }
        public SpellUser spellUser { get; private set; }
        public SpellObject spellObject { get; private set; }
        public SpellData spellData { get; private set; }
        
        public static readonly Evt<SpellBtnItemUI> OnSelectSpell = new Evt<SpellBtnItemUI>();


        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                mainPanel.UseSpell(spellIndex);
            });
        }
        
        public void Initialize(BattleActionUI mainPanel_, int index_)
        {
            Debug.Log(gameObject.name);
            spellIndex = index_;
            
            // if (spellUser.spellList.Count <= spellIndex)
            // {
            //     Debug.Log(spellUser.spellList.Count <= index_);
            //     gameObject.SetActive(false);
            //     return;
            // }
            
            mainPanel = mainPanel_;
            spellUser = mainPanel.player.spellUser;

            spellObject = spellUser.spellList[index_];
            spellData = spellObject.spellData;
            buttonTXT.text = spellData.spellName;
            gameObject.SetActive(true);
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            OnSelectSpell.Invoke(this);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
        }
    }
}
