using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    { 
        string Id { get; }
        string Title { get; }
        Rect GraphRect { get; set; }
        Vector2 GraphPosition { get; set; }
        ValuePort GetValuePort(string id);
        FlowPort GetFlowPort(string id);
        
    }

    [Serializable]
    public abstract class Node : INode
    {
        private static Vector2 _intialPosition = new Vector2(0, 0);
        
#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private string id;

        public string Id => id;
        
#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private string title;

        public string Title => title;

#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private Rect graphRect;

        public Rect GraphRect
        {
            get => graphRect;
            set => graphRect = value;
        }

        public Vector2 GraphPosition
        {
            get => graphRect.position;
            set => graphRect.position = value;
        }

        private Dictionary<string, ValuePort> _valuePorts;
        private Dictionary<string, FlowPort> _flowPorts;

        protected Node()
        {
            id = Guid.NewGuid().ToString();

            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            title = found ? data.Name : "";
            graphRect = found ? new Rect(_intialPosition, data.Size) : new Rect(_intialPosition, new Vector2(200, 100));
        }

        public ValuePort GetValuePort(string id)
        {
            if (_valuePorts == null) CachePorts();
                return _valuePorts[id];
        }

        public FlowPort GetFlowPort(string id)
        {
            if (_flowPorts == null) CachePorts();
                return _flowPorts[id];
        }

        private void CachePorts()
        {
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            _valuePorts = new Dictionary<string, ValuePort>(data.ValuePorts.Count);
            _flowPorts = new Dictionary<string, FlowPort>(data.FlowPorts.Count);
            if (!found) return;
            foreach (var valuePort in data.ValuePorts)
            {
                _valuePorts.Add(valuePort.PortId(this), (ValuePort)valuePort.Port(this)); 
            }
            foreach (var flowPort in data.FlowPorts)
            {
                _flowPorts.Add(flowPort.PortId(this), (FlowPort)flowPort.Port(this)); 
            }
        }
    }
}
