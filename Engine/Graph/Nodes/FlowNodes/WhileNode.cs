using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class WhileNode : Node
    {
        [ValueIn] public bool Condition { get; } = false;
        
        [FlowIn]
        private IEnumerable Enter(Flow flow)
        {
            while (flow.Get(Condition))
            {
                yield return True;
            }

            yield return Exit;
        }

        [FlowOut] private ControlPort True;
        [FlowOut] private ControlPort Exit;
    }
}