using System;
using System.Collections;
using BaseCore;
using BattleSystem;
using CustomHelpers;
using Items;
using Items.Inventory;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public enum BattleAction { BasicAttack = 1, Spell = 2, Item = 3, Skip = 4, }
    
    public class BattleActionUI : Singleton<BattleActionUI>
    {
    
        [BoxGroup("Panels")] [SerializeField] private GameObject mainPanel;
        [BoxGroup("Panels")] [SerializeField] private GameObject actionPanel;
        [BoxGroup("Panels")] [SerializeField] private SpellActionPanelUI spellPanel;
        [BoxGroup("Panels")] [SerializeField] private UseItemActionUI itemPanel;
        [BoxGroup("Panels")] [SerializeField] private TargetPanel enemyTargetPanel;
        [BoxGroup("Panels")] [SerializeField] private TargetPanel playerTargetPanel;
        
        [BoxGroup("Buttons")] [SerializeField] private Button itemsBtn;
        
        public BattleManager battleManager { get; private set; }
        public BattleCharacter player { get; private set; }
        public BattleCharacter currentTarget { get; set; } 
        public BattleAction currentAction { get; set; }

        #region Unity Functions

        private void Start()
        {
            BattleManager.OnPlayerTurnStart.AddListener(ShowActionMenu);
            battleManager = BattleManager.Instance;
            enemyTargetPanel.Initialize(this, battleManager.enemyParty);
            playerTargetPanel.Initialize(this, battleManager.playerParty);
            itemPanel.Initialize(this);
            
            mainPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            BattleManager.OnPlayerTurnStart.RemoveListener(ShowActionMenu);
        }

        #endregion

        #region Panel Functions
        
        private void ShowActionMenu(BattleCharacter playerCharacter_)
        {
            itemsBtn.interactable = itemPanel.HasItems();
            player = playerCharacter_;
            currentAction = BattleAction.BasicAttack;
            currentTarget = battleManager.GetFirstAliveEnemy();
            mainPanel.SetActive(true);
            BackToActionPanel();
            BattleTextManager.Stop();
        }
        
        public void CloseOtherPanels()
        {
            actionPanel.SetActive(false);
            spellPanel.gameObject.SetActive(false);
            enemyTargetPanel.gameObject.SetActive(false);
            playerTargetPanel.gameObject.SetActive(false);
            itemPanel.gameObject.SetActive(false);
        }

        public void BackToActionPanel()
        {
            CloseOtherPanels();
            actionPanel.SetActive(true);
        }
        
        public void ShowSpellMenu()
        {
            CloseOtherPanels();
            spellPanel.ShowPanel(this);
        }
        
        public void ShowEnemyTargetPanel()
        {
            CloseOtherPanels();
            enemyTargetPanel.gameObject.SetActive(true);
        }
        
        public void ShowPlayerTargetPanel()
        {
            CloseOtherPanels();
            playerTargetPanel.gameObject.SetActive(true);
        }

        public void ShowItemMenu()
        {
            CloseOtherPanels();
            itemPanel.gameObject.SetActive(true);
        }
        
        #endregion
        
        #region Button Functions

        public void BasicAttackBtnClick()
        {
            currentAction = BattleAction.BasicAttack;
            ShowEnemyTargetPanel();
        }

        #endregion

        #region Actions
        
        public void StartAction(BattleCharacter target_)
        {
            switch (currentAction)
            {
                case BattleAction.BasicAttack:
                    Attack(target_);
                    break;
                case BattleAction.Spell:
                    UseSpell(target_);
                    break;
                case BattleAction.Item:
                    UseItem(target_);
                    break;
                case BattleAction.Skip:
                    SkipTurn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Attack(BattleCharacter target_)
        {
            if (player == null)
            {
                Debug.LogWarning("PLAYER IS NULL");
                BattleManager.OnPlayerEndDecide.Invoke();
            }
            mainPanel.SetActive(false);
            StartCoroutine(Co_Attack(target_));
        }

        private IEnumerator Co_Attack(BattleCharacter target_)
        {
            yield return player.AttackTarget(target_);
            BattleManager.OnPlayerEndDecide.Invoke();
        }
        
        private void UseSpell(BattleCharacter target_)
        {
            if (player == null)
            {
                Debug.LogWarning("PLAYER IS NULL");
                BattleManager.OnPlayerEndDecide.Invoke();
            }
            
            if (!spellPanel.IsSpellIndexValid(SpellBtnItemUI.currIndex)) return;
            
            mainPanel.SetActive(false);

            StartCoroutine(Co_Spell(target_));
        }
    
        private IEnumerator Co_Spell(BattleCharacter target_)
        {
            yield return player.spellUser.UseSpell(SpellBtnItemUI.currIndex, target_);
            BattleManager.OnPlayerEndDecide.Invoke();
        }
        
        private void UseItem(BattleCharacter target_)
        {
            if (UseItemActionUI.CurrentItemBtn == null ||
                UseItemActionUI.CurrentItemBtn.item == null)
            {
                itemPanel.UpdateInventory(null);
                ShowActionMenu(player);
                return;
            }
            
            CloseOtherPanels();
            mainPanel.SetActive(false);
            
            StartCoroutine(Co_Item(target_));
        }
        
        private IEnumerator Co_Item(BattleCharacter target_)
        {
            var currentItem = UseItemActionUI.CurrentItemBtn.item;
            
            currentItem.Consume(target_.character.statusEffectReceiver);
            if(currentItem is ItemConsumable _itemConsumable) InventoryEvents.OnUpdateStackable.Invoke(_itemConsumable);

            yield return CoroutineHelper.GetWait(1f);
            yield return null;
            BattleManager.OnPlayerEndDecide.Invoke();
        }
        
        public void SkipTurn()
        {
            mainPanel.SetActive(false);
            StartCoroutine(Co_SkipTurn());
        }

        private IEnumerator Co_SkipTurn()
        {
            Debug.Log("Player Skipped a Turn");
            yield return CoroutineHelper.GetWait(0.2f);
            BattleManager.OnPlayerEndDecide.Invoke();
        }

        public void Flee()
        {
            battleManager.TryFlee();
            mainPanel.SetActive(false);
        }

        #endregion
    }
}
