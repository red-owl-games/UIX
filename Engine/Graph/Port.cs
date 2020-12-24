using System;
using System.Collections;
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
        InOut,
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

        public override string ToString()
        {
            return $"{node}.{port}";
        }
    }

    [Serializable]
    public abstract class Port
    {
        [SerializeField]
        protected PortId id;

        public PortId Id => id;
        
        // public string NodeId => id.Node;
        public string PortId => id.Port;
        
        public string Name { get; protected set; }

        public PortDirection Direction { get; protected set; }
         
        public PortCapacity Capacity { get; protected set; }
        
        [SerializeField]
        private List<PortId> connections;

        public List<PortId> Connections => connections;
        
        protected Port(INode node)
        {
            id = new PortId(node.NodeId, Guid.NewGuid().ToString());
            connections = new List<PortId>();
        }
        
        public void Connect(PortId output) => connections.Add(output);
        public void Disconnect(PortId output) => connections.Remove(output);

        public override string ToString() => $"[{Name} | {Direction} | {Capacity}]";

        public static implicit operator PortId(Port port) => port.id;

        public abstract IEnumerator Execute();
    }

    public abstract class Port<T> : Port where T : IPortReflectionData
    {
        protected Port(INode node) : base(node) {}

        public abstract void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , T portData) where TNode : INode;
    }
}
