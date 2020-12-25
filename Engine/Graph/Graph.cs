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
    public interface IGraph : INode
    {
        INode GetNode(string id);
        IEnumerable<T> GetNodes<T>() where T : INode;
        IEnumerable<INode> Nodes { get; }
        int NodeCount { get; }
        T Add<T>(T node) where T : INode;
        bool Get(string id, out INode node);
        void Remove<T>(T node) where T : INode;

        void Connect(IPort output, IPort input);
        void Disconnect(IPort output, IPort input);

        IValuePort GetValuePort(PortId id, PortDirection direction);
        IFlowPort GetFlowPort(PortId id, PortDirection direction);
    }

    [Graph]
    [Node("Common", Name = "SubGraph", Path = "Common")]
    public class Graph : Node, IGraph
    {
        [SerializeReference] 
        private List<INode> _nodes;

        public IEnumerable<INode> Nodes => _nodes;

        public int NodeCount => _nodes.Count;

        public Graph()
        {
            _nodes = new List<INode>();
        }

        protected override void OnInitialize()
        {
            foreach (var node in _nodes)
            {
                node.Initialize();
            }
        }

        public INode GetNode(string id)
        {
            foreach (var node in _nodes)
            {
                if (node.NodeId == id) return node;
            }

            return null;
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
            if (IsInitialized) node.Initialize();
            return node;
        }

        public bool Get(string id, out INode node)
        {
             foreach (var n in _nodes)
             {
                 if (n.NodeId != id) continue;
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
                if (n.NodeId == node.NodeId)
                {
                    CleanupFlowPortConnections(n);
                    CleanupValuePortConnections(n);
                    _nodes.RemoveAt(i);
                }
            }
        }

        private void CleanupFlowPortConnections(INode target)
        {
            foreach (var node in _nodes)
            {
                if (node.NodeId == target.NodeId) continue;
                foreach (var output in node.FlowOutPorts.Values)
                {
                    foreach (var input in output.Connections)
                    {
                        if (target.FlowInPorts.TryGetValue(output.Id.Port, out var _))
                        {
                            output.Disconnect(input);
                        }
                    }
                }
            }
        }

        private void CleanupValuePortConnections(INode target)
        {
            foreach (var node in _nodes)
            {
                if (node.NodeId == target.NodeId) continue;
                foreach (var input in node.ValueInPorts.Values)
                {
                    foreach (var output in input.Connections)
                    {
                        if (target.ValueOutPorts.TryGetValue(output.Port, out var _))
                        {
                            input.Disconnect(output);
                        }
                    }
                }
            }
        }

        // Value Ports - Input -> [Output, Output]
        // Flow Port -  Output -> [Input, Input]
        public void Connect(IPort output, IPort input)
        {
            if (input is ValuePort valueIn && output is ValuePort valueOut)
                valueIn.Connect(valueOut);
            if (input is FlowPort flowIn && output is FlowPort flowOut) 
                flowOut.Connect(flowIn);
        }

        public void Disconnect(IPort output, IPort input)
        {
            if (input is ValuePort valueIn && output is ValuePort valueOut)
                valueIn.Disconnect(valueOut);
            if (input is FlowPort flowIn && output is FlowPort flowOut)
                flowOut.Disconnect(flowIn);
        }

        public IValuePort GetValuePort(PortId id, PortDirection direction)
        {
            var node = GetNode(id.Node);
            switch (direction)
            {
                case PortDirection.Input:
                    return node.ValueInPorts[id.Port];
                case PortDirection.Output:
                    return node.ValueOutPorts[id.Port];
                default:
                    return null;
            }
        }

        public IFlowPort GetFlowPort(PortId id, PortDirection direction)
        {
            var node = GetNode(id.Node);
            switch (direction)
            {
                case PortDirection.Input:
                    return node.FlowInPorts[id.Port];
                case PortDirection.Output:
                    return node.FlowOutPorts[id.Port];
                default:
                    return null;
            }
        }
    }
}
