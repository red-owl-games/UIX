namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsRootNode = true)]
    public class UpdateNode : Node
    {
        [FlowOut] public FlowPort Update;

        public UpdateNode()
        {
            Update = new FlowPort(this);
        }
        
        public override void Definition()
        {
            
        }
    }
}