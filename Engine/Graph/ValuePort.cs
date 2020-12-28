using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IValuePort : IPort
    {
        object Current { get;  }
        void Set(IFlow flow, IPort target);
    }
    
    [Serializable]
    public class ValuePort<T> : Port<ValuePortSettings>, IValuePort
    {
        [SerializeField] protected BetterType valueType;
        
        [SerializeField]
        protected T value;

        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public object Current => value;

        public Type ValueType => valueType;

        public ValuePort(INode node, T defaultValue = default) : base(node)
        {
            value = defaultValue;
            valueType = typeof(T);
        }
        
        public ValuePort(IValuePort port) : base(port)
        {
            value = (T)port.Current;
            valueType = port.ValueType;
        }
        
        public override void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , ValuePortSettings portData)
        {
            Name = portData.Name;
            Direction = portData.Direction;
            Capacity = portData.Capacity;
        }

        public void Set(IFlow flow, IPort target)
        {
            flow.Set(target, value);
        }

        public static implicit operator T(ValuePort<T> self) => self.value;
    }
    
    internal sealed class ValuePort : ValuePort<object>
    {
        public ValuePort(IValuePort port) : base(port) {}
    }
}