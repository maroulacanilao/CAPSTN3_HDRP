using System.Collections;
using Character;

namespace BattleSystem.BattleState
{
    public abstract class BattleStateBase
    {
        public BattleManager BattleManager { get; protected set; } 
        public BattleStateMachine StateMachine { get; protected set; } 

        public BattleStateBase(BattleStateMachine stateMachine_)
        {
            StateMachine = stateMachine_;
            BattleManager = stateMachine_.battleManager;
        }

        public abstract IEnumerator Enter();

        public abstract IEnumerator Exit();
    }
}