using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class LogNode : Node
    {
        [ValueIn] public ValuePort<string> Message;
        
        [FlowIn(Callback = nameof(OnEnter))] public FlowPort Enter;

        public LogNode() : this("") {}
        public LogNode(string defaultMessage = "")
        {
            Message = new ValuePort<string>(this, defaultMessage);
            Enter = new FlowPort(this);
        }
        
        public override void Definition()
        {
            
        }

        private void OnEnter(IFlow flow) => Debug.Log(Message.Value);
    }
}
