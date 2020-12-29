namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", IsRootNode = true)]
    public class LateUpdateNode : Node
    {
        [FlowOut] public FlowPort LateUpdate;

        public LateUpdateNode()
        {
            LateUpdate = new FlowPort(this);
        }
        
        public override void Definition()
        {
            
        }
    }
}