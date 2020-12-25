using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IValuePort : IPort
    {
        IEnumerator Execute();
    }
    
    [Serializable]
    public class ValuePort : Port<UIXValuePortReflection>, IValuePort, ISerializationCallbackReceiver
    {
        [SerializeField] protected string serializationData;
        
        [SerializeField] protected BetterType valueType;
        
        protected ValuePort(INode node) : base(node) {}

        public ValuePort(IValuePort port) : base(port)
        {
            valueType = port.ValueType;
        }
        
        public Type ValueType => valueType;

        public void Set<T>(IFlow flow, T value)
        {
            flow.Set(this, value);
        }

        protected virtual void Serialize() {}
        public void OnBeforeSerialize() => Serialize();
        protected virtual void Deserialize() {}
        public void OnAfterDeserialize() => Deserialize();

        public override void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , UIXValuePortReflection portData)
        {
            Name = portData.Name;
            Direction = portData.Direction;
            Capacity = portData.Capacity;
            
            //Debug.Log($"Initializing Value Port '{this}' for node '{node}'");
        }

        public IEnumerator Execute()
        {
            throw new NotImplementedException();
        }
    }
    
    [Serializable]
    public struct ValueContainer<T>
    {
        public T Value;
        
        public static implicit operator T(ValueContainer<T> self) => self.Value;
        public static implicit operator ValueContainer<T>(T value) => new ValueContainer<T>{ Value = value };
    }

    [Serializable]
    public class ValuePort<T> : ValuePort
    {
        public ValueContainer<T> DefaultValue { get; set; }
        
        public ValuePort(INode node, T defaultValue = default) : base(node)
        {
            valueType = typeof(T);
            DefaultValue = defaultValue;
        }
        
        public T Get(IFlow flow)
        {
            return flow.Get(this, out T output) ? output : (T)DefaultValue;
        }

        public static implicit operator T(ValuePort<T> self) => self.DefaultValue;
        
        protected override void Serialize()
        {
            serializationData = JsonUtility.ToJson(DefaultValue);
        }

        protected override void Deserialize()
        {
            DefaultValue = JsonUtility.FromJson<T>(serializationData);
        }
    }
}