using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    // TODO: should Ports serialize their direction so we can validate you can connect 
    
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
        
        public string NodeId => id.Node;
        public string PortId => id.Port;
        
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

        public override string ToString()
        {
            return $"{id.Node}.{id.Port}";
        }

        public static implicit operator PortId(Port port) => port.id;

        public static Dictionary<string, FlowPort> GetFlowPorts<T>(T node) where T : INode
        {
            if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) return new Dictionary<string, FlowPort>();
            var output = new Dictionary<string, FlowPort>(data.FlowPorts.Count);
            foreach (var port in data.FlowPorts)
            {
                var flowPort = (FlowPort) port.Port(node);
                
                //flowPort.SetCallback(data.CreateDelegate(flowPort.CallbackName, node));
                output.Add(port.PortId(node), flowPort); 
            }

            return output;
        }

        public static Dictionary<string, ValuePort> GetValuePorts<T>(T node) where T : INode
        {
            if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) return new Dictionary<string, ValuePort>();
            var output = new Dictionary<string, ValuePort>(data.ValuePorts.Count);
            foreach (var port in data.ValuePorts)
            {
                output.Add(port.PortId(node), (ValuePort) port.Port(node)); 
            }

            return output;
        }

        public abstract IEnumerator Execute();
    }
}
