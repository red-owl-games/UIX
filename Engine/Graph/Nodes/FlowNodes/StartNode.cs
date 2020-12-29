using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", Deletable = false, Moveable = false, IsRootNode = true)]
    public class StartNode : Node
    {
        [FlowOut] public FlowPort Start;
        
        public StartNode()
        {
            Start = new FlowPort(this);
        }
        
        public override void Definition()
        {
            
        }
    }
}