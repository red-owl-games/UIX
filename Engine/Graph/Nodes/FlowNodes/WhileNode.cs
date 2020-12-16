using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class WhileNode : Node
    {
        [ValueIn] public ValuePort Condition;
        

        [FlowIn] public FlowPort Enter;
        [FlowOut] private FlowPort True;
        [FlowOut] private FlowPort Exit;

        public WhileNode()
        {
            Condition = new ValuePort<bool>(this, true);

            Enter = new FlowPort(this, nameof(OnEnter));

            True = new FlowPort(this);
            Exit = new FlowPort(this);
        }
        
        
        private IEnumerable OnEnter(Flow flow)
        {
            while (flow.Get<bool>(Condition))
            {
                yield return True;
            }

            yield return Exit;
        }


    }
}