namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsFlowRoot = true)]
    public class LateUpdateNode : FlowNode
    {
        [FlowOut] public FlowPort LateUpdate;
    }
}