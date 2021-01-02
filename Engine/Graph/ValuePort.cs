using System;
using UnityEngine.Scripting;

namespace RedOwl.UIX.Engine
{
    public interface IValuePort : IPort
    {
        object WeakValue { get;  }
        void Definition(INode node, ValuePortSettings settings);
        void Set(IFlow flow, IPort target);
    }

    [Preserve]
    public class ValuePort<T> : Port, IValuePort
    {
        public object WeakValue { get; private set; }
        
        // TODO: this should get data from the current "flow" otherwise return the "defaultValue"
        public T Value
        {
            get => (T)WeakValue;
            set => WeakValue = value;
        }

        public Type ValueType { get; protected set; }
        
        [Preserve]
        public ValuePort() {}

        public ValuePort(T defaultValue)
        {
            Value = defaultValue;
        }
        
        public void Definition(INode node, ValuePortSettings settings)
        {
            Id = new PortId(node.NodeId, settings.Name);
            Name = settings.Name;
            Direction = settings.Direction;
            Capacity = settings.Capacity;
            ValueType = typeof(T);

        }
        
        public void Set(IFlow flow, IPort target)
        {
            flow.Set(target, WeakValue);
        }

        public static implicit operator T(ValuePort<T> self) => self.Value;
        public static implicit operator ValuePort<T>(T self) => new ValuePort<T>(self);
    }
}