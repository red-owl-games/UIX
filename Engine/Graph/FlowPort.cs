using System;
using System.Collections;

namespace RedOwl.UIX.Engine
{
    public interface IFlowPort : IPort
    {
        IEnumerator Execute(IFlow flow);
    }
    
    [Serializable]
    public class FlowPort : Port<UIXFlowPortReflection>, IFlowPort
    {
        public FlowPort(INode node) : base(node) {}
        public FlowPort(IFlowPort port) : base(port) {}

        public override void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , UIXFlowPortReflection portData)
        {
            Name = portData.Name;
            Direction = portData.Direction;
            Capacity = portData.Capacity;
            // .SetCallback(nodeData.CreateDelegate(portData.CallbackName, node));
            // private Type asyncType = typeof(Func<IEnumerator>);
            // public Func<IEnumerator> CreateDelegate<T>(string callback, T node) where T : INode
            // {
            //     return (Func<IEnumerator>) _methods[callback].CreateDelegate(asyncType, node);
            // }
            
            //Debug.Log($"Initializing Flow Port '{this}' for node '{node}'");
        }

        public IEnumerator Execute(IFlow flow)
        {
            // TODO: Implement
            yield break;
        }

        private static Type _valueType = typeof(FlowPort);
        public Type ValueType => _valueType;
    }
}