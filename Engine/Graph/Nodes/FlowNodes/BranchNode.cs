namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class BranchNode : Node
    {
        [ValueIn] public bool Condition { get; } = false;
        
        [FlowIn]
        private ControlPort Enter(Flow flow) => flow.Get(Condition) ? A : B;

        [FlowOut] private ControlPort A;
        [FlowOut] private ControlPort B;
    }
}