// using System;
// using System.Collections.Generic;
// using UnityEngine;
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IGraph
    {
        IEnumerable<T> GetNodes<T>() where T : INode;
        IEnumerable<INode> Nodes { get; }
        T Add<T>(T node) where T : INode;
        bool Get(string id, out INode node);
        void Remove<T>(T node) where T : INode;

        ConnectionsGraph ValueConnections { get; }
        ConnectionsGraph FlowConnections { get; }
        
        void Connect(Port output, Port input);
        void Disconnect(Port output, Port input);
        
    }

    [Graph]
    [Node("Common", Name = "SubGraph", Path = "Common")]
    public class Graph : Node, IGraph
    {
        [SerializeReference] 
        private List<INode> _nodes;

        public IEnumerable<INode> Nodes => _nodes;
        
        // TODO: is this right - or should we always just support multiple? Might need to remove configuring capacity of ports then
        // Input Port -> Output Port
        [SerializeField] 
        private ConnectionsGraph _valueConnections;

        public ConnectionsGraph ValueConnections => _valueConnections;
        
        // Output Port -> [Input Port, Input Port]
        [SerializeField] 
        private ConnectionsGraph _flowConnections;

        public ConnectionsGraph FlowConnections => _flowConnections;

        public Graph()
        {
            _nodes = new List<INode>();
            _valueConnections = new ConnectionsGraph();
            _flowConnections = new ConnectionsGraph();
        }
        
        public IEnumerable<T> GetNodes<T>() where T : INode
        {
            foreach(var node in GetNodes(typeof(T)))
            {
                yield return (T) node;
            }
        }

        // TODO: this might not be needed
        public IEnumerable GetNodes(Type nodeType)
        {
            foreach (var node in _nodes)
            {
                if (nodeType.IsInstanceOfType(node)) 
                    yield return node;
            }
        }
        
        public T Add<T>(T node) where T : INode
        {
            _nodes.Add(node);
            return node;
        }

        public bool Get(string id, out INode node)
        {
             foreach (var n in _nodes)
             {
                 if (n.Id != id) continue;
                 node = n;    
                 return true;
             }

             node = null;
             return false;
        }

        public void Remove<T>(T node) where T : INode
        {
            // TODO: Make sure to remove connections
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                var n = _nodes[i];
                if (n.Id == node.Id) _nodes.RemoveAt(i);
            }
        }
        
        // TODO: Ports should serialize their direction so we can validate you can connect 

        public void Connect(Port output, Port input)
        {
            switch (output)
            {
                case ValuePort valueOutput when input is ValuePort valueInput:
                    Connect(valueOutput, valueInput);
                    break;
                case FlowPort flowOutput when input is FlowPort flowInput:
                    Connect(flowOutput, flowInput);
                    break;
            }
        }
        
        public void Disconnect(Port output, Port input)
        {
            // TODO: there is a bug with disconnecting ports after a Deserialize of the _valueConnections & _flowConnections
            // I think its because we are using "Port" as the key
            switch (output)
            {
                case ValuePort valueOutput when input is ValuePort valueInput:
                    Disconnect(valueOutput, valueInput);
                    break;
                case FlowPort flowOutput when input is FlowPort flowInput:
                    Disconnect(flowOutput, flowInput);
                    break;
            }
        }

        private void Connect(ValuePort output, ValuePort input)
        {
            // TODO: check if output.PortType isCastable to input.PortType
            _valueConnections.Connect(input, output);
        }
        
        private void Disconnect(ValuePort output, ValuePort input)
        {
            _valueConnections.Disconnect(input, output);
        }

        private void Connect(FlowPort output, FlowPort input)
        {
            _flowConnections.Connect(output, input);
        }
        
        private void Disconnect(FlowPort output, FlowPort input)
        {
            _flowConnections.Disconnect(output, input);
        }
    }
}
