using System.Collections;
using Character;
using CustomHelpers;
using Managers;
using UnityEngine;
using System.Linq;

namespace Player.ControllerState
{
    public class AttackInputState : PlayerInputState
    {
        private bool isAttacking;
        private float xInputDir;
        private float xAnimDir;
        private bool isRight;
        private Animator animator;

        public AttackInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            animator = player.animator;
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetFloat(player.attackSpeedHash, player.attackSpeed);
            
            player.animationEventReceiver.AddListener(player.attackHitEvent, OnAttackHit);
            player.animationEventReceiver.AddListener(player.attackEndEvent, OnAttackEnd);
            xInputDir = StateMachine.input.x;
            xAnimDir = player.moveDirection.x;
            player.rb.velocity = Vector3.zero;
            StateMachine.canMove = false;
            isAttacking = true;
            Attack();
            player.StartCoroutine(Co_FallBack());

        }

        public override void Exit()
        {
            isAttacking = false;
            player.animationEventReceiver.RemoveListener(player.attackHitEvent, OnAttackHit);
            player.animationEventReceiver.RemoveListener(player.attackEndEvent, OnAttackEnd);
            StateMachine.timeLastAttack = Time.time;
            animator.ResetTrigger(StateMachine.player.attackHash);
            StateMachine.canMove = true;
            player.StopCoroutine(Co_FallBack());
        }

        private void Attack()
        {
            animator.SetFloat(player.ySpeedHash, 0 );
            
            if (xAnimDir.IsApproximately0())
            {
                if (xInputDir.IsApproximately0())
                {
                    animator.SetFloat(player.xSpeedHash, 1);
                    isRight = true;
                }
                else
                {
                    isRight = xInputDir > 0;
                    animator.SetFloat(player.xSpeedHash, isRight ? 1 : -1);
                }
            }
            else
            {
                isRight = xAnimDir > 0;
                animator.SetFloat(player.xSpeedHash, isRight ? 1 : -1);
            }
            
            animator.SetTrigger(StateMachine.player.attackHash);
        }

        private EnemyCharacter GetEnemyInRange()
        {
            var _pos = player.transform.position + player.attackOffset;
            var _cols = Physics.OverlapBox(_pos, player.attackSize, Quaternion.identity, player.enemyLayer);

            foreach (var col in _cols)
            {
                if(col == null) continue;
                Debug.Log($"1 In Range Attack: <color=yellow>{col.gameObject.name}</color>");
            }

            var _res = _cols.Where(_col => _col != null && _col.transform != player.transform).ToList();

            if (_res.Count == 0) return null;
            
            _res.Sort((a, b) =>
            {
                var _a = a.transform.position;
                var _b = b.transform.position;
                var _distA = Vector3.Distance(_a, player.transform.position);
                var _distB = Vector3.Distance(_b, player.transform.position);
                return _distA.CompareTo(_distB);
            });

            foreach (var _col in _res)
            {
                if(_col.gameObject == null) continue;
                
                Debug.Log($"2 In Range Attack: <color=yellow>{_col.gameObject.name}</color>");
                
                if(_col.TryGetComponent(out EnemyCharacter _enemy)) return _enemy;
            }

            return null;

            // foreach (var _col in _cols)
            // {
            //     if (_col == null) continue;
            //     
            //     var enemy_ = _col.GetComponent<EnemyCharacter>();
            //     if (enemy_ != null) return enemy_;
            // }
            //
            // return null;
        }

        private void OnAttackHit()
        {
            Debug.Log("ATTACK HIT");
            var _enemy = GetEnemyInRange();
            
            if (_enemy.IsEmptyOrDestroyed())
            {
                Debug.Log("No Enemy");
                return;
            }

            if (isRight && _enemy.transform.position.x < player.transform.position.x)
            {
                Debug.Log($"IsRight: {isRight} | Enemy: {_enemy.transform.position.x} | Player: {player.transform.position.x}");
                return;
            }
            if (!isRight && _enemy.transform.position.x > player.transform.position.x)
            {
                Debug.Log($"IsRight: {isRight} | Enemy: {_enemy.transform.position.x} | Player: {player.transform.position.x}");
                return;
            }

            // Debug.Log("Hit");
            GameManager.OnEnterBattle.Invoke(_enemy, true);
        }

        private void OnAttackEnd()
        {
            StateMachine.ChangeState(StateMachine.GroundedState);
        }

        private IEnumerator Co_FallBack()
        {
            yield return new WaitForSeconds(2f);
            OnAttackEnd();
        }
    }
}