namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class StartNode : Node
    {
        [FlowOut] private ControlPort Start;
    }
}