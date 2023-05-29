using System;
using System.Collections;
using BattleSystem;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Battle
{
    public enum BattleAction { BasicAttack = 1, Spell = 2, Item = 3, Skip = 4, }
    
    public class BattleActionUI : MonoBehaviour
    {
    
        [BoxGroup("Panels")] [SerializeField] private GameObject mainPanel;
        [BoxGroup("Panels")] [SerializeField] private GameObject actionPanel;
        [BoxGroup("Panels")] [SerializeField] private SpellActionPanelUI spellPanel;
        [BoxGroup("Panels")] [SerializeField] private TargetPanel enemyTargetPanel;
        [BoxGroup("Panels")] [SerializeField] private TargetPanel playerTargetPanel;
        
        public BattleManager battleManager { get; private set; }
        public BattleCharacter player { get; private set; }
        public BattleCharacter currentTarget { get; set; } 
        public BattleAction currentAction { get; set; }

        #region Unity Functions

        private void Awake()
        {
            BattleManager.OnPlayerTurnStart.AddListener(ShowActionMenu);
            battleManager = BattleManager.Instance;
            enemyTargetPanel.Initialize(this, battleManager.enemyParty);
            playerTargetPanel.Initialize(this, battleManager.playerParty);
        }

        private void OnDestroy()
        {
            BattleManager.OnPlayerTurnStart.RemoveListener(ShowActionMenu);
        }

        #endregion

        #region Panel Functions
        
        private void ShowActionMenu(BattleCharacter playerCharacter_)
        {
            player = playerCharacter_;
            currentAction = BattleAction.BasicAttack;
            currentTarget = battleManager.GetFirstAliveEnemy();
            mainPanel.SetActive(true);
            BackToActionPanel();
        }
        
        public void CloseOtherPanels()
        {
            actionPanel.SetActive(false);
            spellPanel.gameObject.SetActive(false);
            enemyTargetPanel.gameObject.SetActive(false);
            playerTargetPanel.gameObject.SetActive(false);
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
        
        #endregion
    }
}
