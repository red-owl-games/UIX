using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    {
        string NodeId { get; }
        string NodeTitle { get; }
        Rect NodeRect { get; set; }
        Vector2 NodePosition { get; set; }

        // TODO: Convert to KeyedCollection based on performance hit of using Dictionary.Values - which seems to create a ValueCollection every time?
        Dictionary<string, IValuePort> ValueInPorts { get; }
        Dictionary<string, IValuePort> ValueOutPorts { get; }
        
        bool IsDefined { get; }
        void Definition();
    }

    public interface IFlowNode : INode
    {
        bool IsFlowRoot { get; }
        
        Dictionary<string, IFlowPort> FlowInPorts { get; }
        Dictionary<string, IFlowPort> FlowOutPorts { get; }
        
        bool IsInitialized { get; }
        void Initialize();
    }

    [Serializable]
    public abstract class Node : INode 
    {
        protected static Vector2 DefaultPosition = new Vector2(0, 0);

#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private string nodeId = Guid.NewGuid().ToString();

        public string NodeId => nodeId;

#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private Rect nodeRect;

        public Rect NodeRect
        {
            get => nodeRect;
            set => nodeRect = value;
        }

        public Vector2 NodePosition
        {
            get => nodeRect.position;
            set => nodeRect.position = value;
        }
        
        public string NodeTitle { get; protected set; }

        public Dictionary<string, IValuePort> ValueInPorts { get; } = new Dictionary<string, IValuePort>();
        public Dictionary<string, IValuePort> ValueOutPorts { get; } = new Dictionary<string, IValuePort>();

        [field: NonSerialized] // Prevents a weird editor domain reload bug where IsDefined stays true
        public bool IsDefined { get; private set; }

        protected Node()
        {
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            NodeRect = found ? new Rect(DefaultPosition, data.Size) : new Rect(DefaultPosition, new Vector2(200, 100));
            NodeTitle = found ? data.Name : "";
        }

        public void Definition()
        {
            if (IsDefined) return;
            try
            {
                InternalDefinition();
                OnDefinition();
                IsDefined = true;
            }
            catch
            {
                Debug.LogWarning($"Failed to Define Node {GetType().FullName} | {NodeId}");
                throw;
            }
        }

        internal virtual void InternalDefinition()
        {
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            if (!found) return;
            foreach (var valuePort in data.ValuePorts)
            {
                switch (valuePort.Direction)
                {
                    case PortDirection.Input:
                        ValueInPorts.Add(valuePort.Name, valuePort.GetOrCreatePort(this));
                        break;
                    case PortDirection.Output:
                        ValueOutPorts.Add(valuePort.Name, valuePort.GetOrCreatePort(this));
                        break;
                }
                //Debug.Log($"{NodeTitle}Node has Value Port '{valuePort.Name} | {valuePort.Direction}'");
            }
        }

        protected virtual void OnDefinition() {}

        public override string ToString() => $"{NodeTitle}Node";

        // public static Port Port(string id = null)
        // {
        //     var port = new Port(id);
        //     // TODO: Add port to proper Dictionary
        //     return port;
        // }
    }

    public abstract class FlowNode : Node, IFlowNode
    {
        [field: NonSerialized] // Prevents a weird editor domain reload bug where IsInitialized stays true
        public bool IsInitialized { get; private set; }

        public bool IsFlowRoot { get; private set; } = false;
        
        public Dictionary<string, IFlowPort> FlowInPorts { get; } = new Dictionary<string, IFlowPort>();
        public Dictionary<string, IFlowPort> FlowOutPorts { get; } = new Dictionary<string, IFlowPort>();
        
        internal override void InternalDefinition()
        {
            base.InternalDefinition();
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            if (!found) return;
            foreach (var flowPort in data.FlowPorts)
            {
                switch (flowPort.Direction)
                {
                    case PortDirection.Input:
                        FlowInPorts.Add(flowPort.Name, flowPort.GetOrCreatePort(this));
                        break;
                    case PortDirection.Output:
                        FlowOutPorts.Add(flowPort.Name, flowPort.GetOrCreatePort(this));
                        break;
                }
                //Debug.Log($"{NodeTitle}Node has Flow Port '{flowPort.Name} | {flowPort.Direction}'");
            }
        }

        public void Initialize()
        {
            if (IsInitialized) return;
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            IsFlowRoot = found ? data.IsFlowRoot : false;
            try
            {
                OnInitialize();
                IsInitialized = true;
            }
            catch
            {
                Debug.LogWarning($"Failed to Initialize Node {this} | {NodeId}");
                throw;
            }
        }
        
        protected virtual void OnInitialize() {}
    }
}
