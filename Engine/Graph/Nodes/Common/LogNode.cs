using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class LogNode : FlowNode
    {
        [FlowIn(nameof(OnEnter))] public FlowPort Enter;
        
        [ValueIn] public ValuePort<string> Message = "";

        private void OnEnter(IFlow flow) => Debug.Log(Message.Value);
    }
}
