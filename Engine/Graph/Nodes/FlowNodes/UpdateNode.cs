namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class UpdateNode : Node
    {
        [FlowOut] private ControlPort Update;
    }
}