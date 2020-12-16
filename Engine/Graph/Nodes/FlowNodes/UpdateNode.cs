namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class UpdateNode : Node
    {
        [FlowOut] public FlowPort Update;

        public UpdateNode()
        {
            Update = new FlowPort(this);
        }
    }
}