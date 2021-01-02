using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Flow", Path = "Flow Control", Deletable = false, Moveable = false, IsFlowRoot = true)]
    public class StartNode : FlowNode
    {
        [FlowOut] public FlowPort Start;
    }
}