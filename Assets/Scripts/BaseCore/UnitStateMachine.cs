namespace BaseCore
{
    public abstract class UnitStateMachine
    {
        public UnitState CurrentState { get; protected set; }
    
        public abstract void Initialize();

        public abstract void StateUpdate();
    
        public abstract void StateFixedUpdate();
    
        public void ChangeState(UnitState newState_)
        {
            CurrentState?.Exit();

            CurrentState = newState_;
            CurrentState?.Enter();
        }
    }
}