using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public class ValuePort : Port, ISerializationCallbackReceiver
    {
        [SerializeField] protected string serializationData;
        
        [SerializeField] protected BetterType portType;
        
        protected ValuePort(INode node) : base(node) {}
        
        public Type PortType => portType;

        // public object Get(IFlow flow)
        // {
        //     // TODO: pull from flow - if not present return value
        //     return null;
        // }
        
        public void Set<T>(IFlow flow, T value)
        {
            flow.Set(this, value);
        }

        protected virtual void Serialize() {}
        public void OnBeforeSerialize() => Serialize();
        protected virtual void Deserialize() {}
        public void OnAfterDeserialize() => Deserialize();
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
            portType = typeof(T);
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