using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public class FlowPort : Port<UIXFlowPortReflection>
    {
        public FlowPort(INode node) : base(node) {}

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

        public override IEnumerator Execute() => throw new NotImplementedException();
    }
}