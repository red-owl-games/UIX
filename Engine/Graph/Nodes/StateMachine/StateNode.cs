namespace RedOwl.UIX.Engine
{
    public abstract class StateNode : Node
    {
        [FlowIn] public FlowPort Enter;
        [FlowIn] public FlowPort Update;
        [FlowOut] public FlowPort Exit;

        protected StateNode()
        {
            Enter = new FlowPort(this, nameof(OnEnter));
            Update = new FlowPort(this, nameof(OnUpdate));
            Exit = new FlowPort(this, nameof(OnExit));
        }

        protected abstract void OnEnter();
        protected abstract void OnUpdate();
        protected abstract void OnExit();
    }
}