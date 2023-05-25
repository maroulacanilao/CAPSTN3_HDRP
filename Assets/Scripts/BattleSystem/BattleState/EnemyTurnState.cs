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
        EnemyData enemyData;
        
        public EnemyTurnState(BattleStateMachine stateMachine_, BattleCharacter battleCharacter_) : base(stateMachine_, battleCharacter_)
        {
            enemyData = battleCharacter.characterData as EnemyData;
        }

        public override IEnumerator Enter()
        {
            if (!battleCharacter.character.IsAlive)
            {
                StateMachine.NextTurnState();
            }
            Debug.Log($"{battleCharacter.gameObject}'s Enter Turn");
            yield return CoroutineHelper.GetWait(0.1f);
            yield return battleCharacter.character.statusEffectReceiver.BeforeTurnTick(this);
            yield return StartTurn();
        }

        public override IEnumerator StartTurn()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Turn");
            yield return CheckForEndState();
            yield return CoroutineHelper.GetWait(.2f);
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            // var _action = enemyData.combatTendency.GetAction(battleCharacter.character);
            // BattleCharacter _target;
            // yield return null;
            //
            // switch (_action.actionType)
            // {
            //     case AIActionType.BasicAttack:
            //
            //         break;
            //     
            //     case AIActionType.SpellAttack:
            //     case AIActionType.DeBuff:
            //         _target = BattleManager.playerParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
            //         yield return battleCharacter.spellUser.UseSkill(_action.spellData, _target);
            //         break;
            //     
            //     case AIActionType.Buff:
            //     case AIActionType.Heal:
            //         _target = BattleManager.enemyParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
            //         yield return battleCharacter.spellUser.UseSkill(_action.spellData, _target);
            //         break;
            // }
            
            var _target = BattleManager.playerParty.Where(p => p.character.IsAlive).ToArray().GetRandomItem();
            yield return battleCharacter.AttackTarget(_target);
            
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(0.1f);
            yield return battleCharacter.character.statusEffectReceiver.AfterTurnTick(this);
            Debug.Log($"{battleCharacter.gameObject}'s End Turn");
            
            yield return CheckForEndState(true);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Exit State");
            yield break;
        }
    }
}