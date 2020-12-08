using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    {
        string Id { get; }
        Vector2 Position { get; set; }
        
        IEnumerable<ValuePort> ValuePorts { get; }
        
        IEnumerable<FlowPort> FlowPorts { get; }

        ValuePort GetValuePort(uint id);
        FlowPort GetFlowPort(uint id);
    }
    
    [Serializable]
    public abstract class Node : INode
    {
        
        [field: HideInInspector]
        [field: SerializeField] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [field: HideInInspector]
        [field: SerializeField]
        public Vector2 Position { get; set; }

        private Dictionary<uint, ValuePort> _valuePorts;
        public IEnumerable<ValuePort> ValuePorts
        {
            get
            {
                if (_valuePorts == null) CachePorts();
                foreach (var port in _valuePorts.Values)
                {
                    yield return port;
                }
            }
        }

        private Dictionary<uint, FlowPort> _flowPorts;
        public IEnumerable<FlowPort> FlowPorts
        {
            get
            {
                if (_flowPorts == null) CachePorts();
                foreach (var port in _flowPorts.Values)
                {
                    yield return port;
                }
            }
        }
        
        
        public ValuePort GetValuePort(uint id)
        {
            if (_valuePorts == null) CachePorts();
            return _valuePorts[id];
        }

        public FlowPort GetFlowPort(uint id)
        {
            if (_flowPorts == null) CachePorts();
            return _flowPorts[id];
        }

        private void CachePorts()
        {
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            _valuePorts = new Dictionary<uint, ValuePort>(data.ValuePorts.Count);
            _flowPorts = new Dictionary<uint, FlowPort>(data.FlowPorts.Count);
            if (!found) return;
            foreach (var valuePort in data.ValuePorts)
            {
               _valuePorts.Add(valuePort.Hash, valuePort); 
            }
            foreach (var flowPort in data.FlowPorts)
            {
                _flowPorts.Add(flowPort.Hash, flowPort); 
            }
        }
    }
}