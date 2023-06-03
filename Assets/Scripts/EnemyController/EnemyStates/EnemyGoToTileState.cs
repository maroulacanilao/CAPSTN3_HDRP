using System.Collections;
using Farming;
using Pathfinding;
using UnityEngine;
using CustomHelpers;
using Managers;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyChaseTileState : EnemyControllerState
    {
        private readonly Collider targetCol;
        
        private Vector3 targetPos;
        public EnemyChaseTileState(EnemyAIController aiController_, EnemyStateMachine stateMachine_, FarmTile target_) : base(aiController_, stateMachine_)
        {
            targetTile = target_;
            if(targetTile == null) return;
            targetCol = targetTile.GetComponent<Collider>();
        }

        public override void Enter()
        {
            base.Enter();
            if (targetTile == null)
            {
                DefaultState();
                return;
            }
            
            controller.StopAllCoroutines();
            stateMachine.targetDestination = default;

            controller.aiPath.maxSpeed = controller.movementSpeed;
            controller.aiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            
            var _position = controller.transform.position;
            
            targetPos = targetTile.transform.position;
            
            var _leftDist = Vector3.Distance(_position, targetPos.AddX(-1.5f));
            var _rightDist = Vector3.Distance(_position, targetPos.AddX(1.5f));
            
            if (_leftDist > _rightDist)
            {
                // if right is nearer
                stateMachine.targetDestination = targetPos.AddX(1.5f);
                stateMachine.isTileOnRight = true;
            }
            else
            {
                stateMachine.targetDestination = targetPos.AddX(-1.5f);
                stateMachine.isTileOnRight = false;
            }
            
            controller.animator.SetTrigger(controller.GroundedHash);
            controller.aiPath.destination = stateMachine.targetDestination;
            controller.StartCoroutine(Co_IsWithinRange());
        }
        
        public override void Exit()
        {
            base.Exit();
            controller.StopAllCoroutines();
            controller.animator.ResetTrigger(controller.GroundedHash);
        }
        
        private IEnumerator Co_IsWithinRange(float refreshRate_ = 1f)
        {
            var _waiter = new WaitForSeconds(refreshRate_);

            var _colTransform = controller.attackRangeCollider.transform;
            var _colPos = _colTransform.localPosition;
            
            var _xCol = Mathf.Abs(_colPos.x);
            var _newX = stateMachine.isTileOnRight ? -_xCol : _xCol;
            
            _colTransform.localPosition = _colPos.SetX(_newX);

            while (controller.gameObject.activeSelf)
            {
                yield return _waiter;

                if (targetTile.IsEmptyOrDestroyed())
                {
                    DefaultState();
                    yield break;
                }

                if (targetTile.tileState == TileState.Empty)
                {
                    DefaultState();
                    yield break;
                }
                
                if(IsWithinAttackRange(targetCol)) yield break;
                
                controller.aiPath.destination = stateMachine.targetDestination;
            }
            
            
            stateMachine.ChangeState(new EnemyAttackTileState(controller,stateMachine,targetTile));
        }
    }
}