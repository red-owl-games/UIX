using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class BranchNode : Node
    {
        [ValueIn] public ValuePort Condition;

        [FlowIn] public FlowPort Enter;

        [FlowOut] public FlowPort True;
        [FlowOut] public FlowPort False;
        
        public BranchNode()
        {
            Condition = new ValuePort<bool>(this, false);
            Enter = new FlowPort(this, nameof(OnEnter));
            
            True = new FlowPort(this);
            False = new FlowPort(this);
        }
        
        private IEnumerable OnEnter(IFlow flow)
        {
            yield return flow.Get<bool>(Condition) ? True : False;
        }
    }
}
