using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class LogNode : Node
    {
        [ValueIn] public float Message { get; } = 0;
        
        [FlowIn]
        private void Enter(Flow flow) => Debug.Log(flow.Get(Message));
    }
}