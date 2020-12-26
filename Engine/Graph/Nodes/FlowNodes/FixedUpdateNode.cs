namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsRootNode = true)]
    public class FixedUpdateNode : Node
    {
        [FlowOut] public FlowPort FixedUpdate;

        public FixedUpdateNode()
        {
            FixedUpdate = new FlowPort(this);
        }
    }
}