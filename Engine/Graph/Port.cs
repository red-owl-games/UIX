using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    // TODO: should Ports serialize their direction so we can validate you can connect
    
    public interface IPortReflectionData {}

    public enum PortDirection
    {
        Input,
        Output,
    }

    public enum PortCapacity
    {
        Single,
        Multi,
    }
    
    public interface IPort
    {
        PortId Id { get; }
        
        string Name { get; }

        PortDirection Direction { get; }
         
        PortCapacity Capacity { get; }
        
        Type ValueType { get; }
    }

    [Serializable]
    public struct PortId : IEquatable<PortId>
    {
        [SerializeField]
        private string node;
    
        public string Node => node;
        
        [SerializeField]
        private string port;
    
        public string Port => port;

        public PortId(string nodeId, string portId)
        {
            node = nodeId;
            port = portId;
        }

        public bool Equals(PortId other) => node == other.node && port == other.port;

        public override bool Equals(object obj) => obj is PortId other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((node != null ? node.GetHashCode() : 0) * 397) ^ (port != null ? port.GetHashCode() : 0);
            }
        }

        public static bool operator ==(PortId left, PortId right) => left.Equals(right);

        public static bool operator !=(PortId left, PortId right) => !left.Equals(right);

        public override string ToString()
        {
            return $"{node}.{port}";
        }
    }

    public abstract class Port
    {
        public PortId Id { get; internal set; }
        
        public INode Node { get; internal set; }
        
        public string Name { get; protected set; }

        public PortDirection Direction { get; protected set; }
         
        public PortCapacity Capacity { get; protected set; }
        
        public override string ToString() => $"[{Name} | {Direction} | {Capacity}]";
    }
}
