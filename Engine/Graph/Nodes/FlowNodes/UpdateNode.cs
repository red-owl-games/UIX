namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsFlowRoot = true)]
    public class UpdateNode : FlowNode
    {
        [FlowOut] public FlowPort Update;
    }
}