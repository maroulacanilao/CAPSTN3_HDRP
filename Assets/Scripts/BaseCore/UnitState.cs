using CustomHelpers;

namespace BaseCore
{
    public abstract class UnitState
    {
        protected string animEndEvent;
    
        public virtual void Enter() { }

        public virtual void HandleInput() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void Exit() { }
    }
}
