using System.Collections;
using Character;

namespace BattleSystem.BattleState
{
    public abstract class BattleStateBase
    {
        protected BattleManager BattleManager;
        protected BattleStateMachine StateMachine;

        public BattleStateBase(BattleStateMachine stateMachine_)
        {
            StateMachine = stateMachine_;
            BattleManager = stateMachine_.battleManager;
        }

        public abstract IEnumerator Enter();

        public abstract IEnumerator Exit();
    }
}