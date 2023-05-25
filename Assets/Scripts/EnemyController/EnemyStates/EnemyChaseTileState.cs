using System.Collections;
using Farming;
using Pathfinding;
using UnityEngine;
using CustomHelpers;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyChaseTileState : EnemyControllerState
    {

        public EnemyChaseTileState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }

        public override void Enter()
        {
            base.Enter();
            controller.StopAllCoroutines();
            stateMachine.targetDestination = default;

            controller.aiPath.maxSpeed = controller.movementSpeed;
            controller.aiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            
            var _position = controller.transform.position;
            stateMachine.targetTile = FarmTileManager.Instance.GetAllNonEmptyTile().GetNearestComponent(_position);
            
            if(stateMachine.targetTile == null)
            {
                DefaultState();
                return;
            }
            
            stateMachine.tileCol = stateMachine.targetTile.GetComponent<Collider>();

            var _tilePos = stateMachine.targetTile.transform.position;
            
            var _leftDist = Vector3.Distance(_position, _tilePos.AddX(-1.5f));
            var _rightDist = Vector3.Distance(_position, _tilePos.AddX(1.5f));

            
            if (_leftDist > _rightDist)
            {
                // if right is nearer
                stateMachine.targetDestination = _tilePos.AddX(1.5f);
                stateMachine.isTileOnRight = true;
            }
            else
            {
                stateMachine.targetDestination = _tilePos.AddX(-1.5f);
                stateMachine.isTileOnRight = false;
            }

            controller.animator.SetTrigger(controller.GroundedHash);
            controller.aiPath.destination = stateMachine.targetDestination;
            controller.StartCoroutine(OnReachDestination());
        }

        public override void Exit()
        {
            base.Exit();
            controller.StopAllCoroutines();
            controller.animator.ResetTrigger(controller.GroundedHash);
        }

        private IEnumerator OnReachDestination()
        {
            yield return Co_IsWithinRange(FarmTileManager.Instance.farmTileLayerMask);
            Debug.Log("Reached Destination");
            stateMachine.ChangeState(stateMachine.attackTileState);
        }
        
        private IEnumerator Co_IsWithinRange(LayerMask layerMask_, float refreshRate_ = 1f)
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

                if (stateMachine.targetTile.IsEmptyOrDestroyed())
                {
                    DefaultState();
                    yield break;
                }

                if (stateMachine.targetTile.tileState == TileState.Empty)
                {
                    DefaultState();
                    yield break;
                }
                
                if(IsWithinAttackRange(stateMachine.tileCol)) yield break;
                
                controller.aiPath.destination = stateMachine.targetDestination;
            }
        }


    }
}