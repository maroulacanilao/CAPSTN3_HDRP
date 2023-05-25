using System;
using Character;
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
        private CharacterMana playerCharacterMana => mainPanel.player.character.mana;

        public static int currIndex = 0;
        public static readonly Evt<SpellBtnItemUI> OnSelectSpell = new Evt<SpellBtnItemUI>();

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                mainPanel.currentAction = BattleAction.Spell;
                currIndex = spellIndex;
                if(spellData.spellType is SpellType.Magical or SpellType.Physical) mainPanel.ShowEnemyTargetPanel();
                else mainPanel.ShowPlayerTargetPanel();
            });
        }
        
        public void Initialize(BattleActionUI mainPanel_, int index_)
        {
            Debug.Log(gameObject.name);
            spellIndex = index_;

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

        private void OnEnable()
        {
            button.interactable = spellData.manaCost <= playerCharacterMana.CurrentMana;
        }
    }
}
