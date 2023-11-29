using System;
using Character;
using Character.CharacterComponents;
using CustomEvent;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Battle
{
    public class SpellBtnItemUI : SelectableMenuButton, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI buttonTXT;
        private BattleActionUI mainPanel;
        public int spellIndex { get; private set; }
        public SpellUser spellUser { get; private set; }
        public SpellObject spellObject { get; private set; }
        public SpellData spellData { get; private set; }
        private CharacterMana playerCharacterMana => mainPanel.player.character.mana;

        public static int currIndex = 0;
        public static readonly Evt<SpellBtnItemUI> OnSelectSpell = new Evt<SpellBtnItemUI>();

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(() =>
            {
                mainPanel.currentAction = BattleAction.Spell;
                currIndex = spellIndex;
                if (spellData.spellType is SpellType.Damage or SpellType.DeBuff)
                {
                    if (spellData.isAOE)
                    {
                        var _target = BattleSystem.BattleManager.Instance.GetOppositePartyOf(mainPanel.player.character, false)[0];
                        mainPanel.StartAction(_target);
                        return;
                    }
                    mainPanel.ShowEnemyTargetPanel();
                }
                else
                {
                    //var _target = BattleSystem.BattleManager.Instance.playerParty[1];
                    //mainPanel.StartAction(_target);
                    mainPanel.ShowPlayerTargetPanel();
                }
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

        public override void SelectButton()
        {
            base.SelectButton();
            OnSelectSpell.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            button.interactable = spellData.manaCost <= playerCharacterMana.CurrentMana;
        }
    }
}
