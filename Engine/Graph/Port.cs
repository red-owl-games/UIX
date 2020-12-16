using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
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
    }

    [Serializable]
    public class Port
    {
        [SerializeField]
        protected PortId id;

        public PortId Id => id;
        
        public string NodeId => id.Node;
        public string PortId => id.Port;
        
        protected Port(INode node)
        {
            id = new PortId(node.Id, Guid.NewGuid().ToString());
        }

        public override string ToString()
        {
            return $"{id.Node}.{id.Port}";
        }

        public static implicit operator PortId(Port port) => port.id;
    }
}
