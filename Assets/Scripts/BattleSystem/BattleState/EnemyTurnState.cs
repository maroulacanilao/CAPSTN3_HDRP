using System;
using System.Collections;
using System.Linq;
using Character;
using CustomHelpers;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class EnemyTurnState : TurnBaseState
    {
        private readonly EnemyData enemyData;
        
        public EnemyTurnState(BattleStateMachine stateMachine_, BattleCharacter battleCharacter_) : base(stateMachine_, battleCharacter_)
        {
            enemyData = battleCharacter.characterData as EnemyData;
        }

        protected override IEnumerator TurnLogic()
        {
            yield return EnemyTurnAction();
            
            yield return EndTurn();
        }

        public IEnumerator EnemyTurnAction()
        {
            AICombatAction _action = new AICombatAction()
            {
                actionType = AIActionType.BasicAttack
            };

            try
            {
                _action = enemyData.combatTendency.GetAction(battleCharacter.character);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            yield return null;
            
            BattleCharacter _target;
            
            switch (_action.actionType)
            {
                case AIActionType.BasicAttack:
                    _target = BattleManager.playerParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
                    yield return battleCharacter.AttackTarget(_target);
                    break;
                
                case AIActionType.SpellAttack:
                case AIActionType.DeBuff:
                    _target = BattleManager.playerParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
                    yield return battleCharacter.spellUser.UseSpell(_action.spellData, _target);
                    break;
                
                case AIActionType.Buff:
                case AIActionType.Heal:
                    _target = BattleManager.enemyParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
                    yield return battleCharacter.spellUser.UseSpell(_action.spellData, _target);
                    break;
            }
        }
    }
}