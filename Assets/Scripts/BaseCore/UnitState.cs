using CustomHelpers;

namespace BaseCore
{
    public abstract class UnitState
    {
        protected int animEndEventHash;
    
        public virtual void Enter() { }

        public virtual void HandleInput() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void Exit() { }
        
        protected virtual void OnAnimationEnd() {}


        protected void AnimationEnd(string animEventId_)
        {
            if(animEventId_.ToHash() != animEndEventHash) return;

            OnAnimationEnd();
        }
    }
}
