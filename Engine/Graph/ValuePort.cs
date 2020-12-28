using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IValuePort : IPort
    {
        object Current { get; }
        IEnumerator Execute(IFlow flow);
    }
    
    [Serializable]
    public class ValuePort<T> : Port<ValuePortSettings>, IValuePort
    {
        [SerializeField] protected BetterType valueType;
        
        [SerializeField]
        private T value;

        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public object Current => value;
        
        public Type ValueType => valueType;

        public ValuePort(INode node, T defaultValue = default) : base(node)
        {
            valueType = typeof(T);
            value = defaultValue;
        }
        
        public ValuePort(IValuePort port) : base(port)
        {
            valueType = port.ValueType;
        }
        
        public override void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , ValuePortSettings portData)
        {
            Name = portData.Name;
            Direction = portData.Direction;
            Capacity = portData.Capacity;
            
            //Debug.Log($"Initializing Value Port '{this}' for node '{node}'");
        }

        public IEnumerator Execute(IFlow flow)
        {
            // TODO: Implement
            yield break;
        }

        public static implicit operator T(ValuePort<T> self) => self.value;
    }
    
    internal sealed class ValuePort : ValuePort<object>
    {
        public ValuePort(IValuePort node) : base(node) {}
    }
}