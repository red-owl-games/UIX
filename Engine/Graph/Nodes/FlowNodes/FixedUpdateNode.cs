namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsFlowRoot = true)]
    public class FixedUpdateNode : FlowNode
    {
        [FlowOut] public FlowPort FixedUpdate;
    }
}