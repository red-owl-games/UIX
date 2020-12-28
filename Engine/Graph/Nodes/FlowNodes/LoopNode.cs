using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control")]
    public class LoopNode : Node
    {
        [FlowIn(Callback = nameof(OnEnter))] public FlowPort Enter;
        [FlowOut] public FlowPort Exit;

        public int count;
        
        public LoopNode()
        {
            Enter = new FlowPort(this);
            Exit = new FlowPort(this);
        }
        
        private IEnumerator OnEnter(IFlow flow)
        {
            int times = 0;
            while (times < count)
            {
                yield return Exit;
                times += 1;
            }
        }
    }
}