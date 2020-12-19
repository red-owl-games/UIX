namespace RedOwl.UIX.Engine
{
    public abstract class StateNode : Node
    {
        [FlowIn(Callback=nameof(OnEnter))] public FlowPort Enter;
        [FlowIn(Callback=nameof(OnUpdate))] public FlowPort Update;
        [FlowOut(Callback=nameof(OnExit))] public FlowPort Exit;

        protected StateNode()
        {
            Enter = new FlowPort(this);
            Update = new FlowPort(this);
            Exit = new FlowPort(this);
        }

        protected abstract void OnEnter(IFlow flow);
        protected abstract void OnUpdate(IFlow flow);
        protected abstract void OnExit(IFlow flow);
    }
}