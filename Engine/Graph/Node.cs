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
        
        void Definition();
    }

    public interface IFlowNode : INode
    {
        bool IsFlowRoot { get; }
        
        Dictionary<string, IFlowPort> FlowInPorts { get; }
        Dictionary<string, IFlowPort> FlowOutPorts { get; }
        
        void Initialize(ref IFlow flow);
    }

    [Serializable]
    public abstract class Node : INode 
    {
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

        // TODO: recheck that this is true
        [field: NonSerialized] // Prevents a weird editor domain reload bug where IsDefined stays true
        public bool IsDefined { get; private set; }

        protected Node()
        {
            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            NodeRect = new Rect(new Vector2(0, 0), found ? data.Size : new Vector2(200, 100));
            NodeTitle = found ? data.Name : "";
        }
        
        private void DefineValuePorts()
        {
            if (!UIXGraphReflector.NodeCache.Get(GetType(), out var data)) return;
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
            DefineValuePorts();
        }

        protected virtual void OnDefinition() {}

        public override string ToString() => $"{NodeTitle}Node";
    }

    public abstract class FlowNode : Node, IFlowNode
    {
        public bool IsFlowRoot => UIXGraphReflector.NodeCache.Get(GetType(), out var data) && data.IsFlowRoot;
        
        public Dictionary<string, IFlowPort> FlowInPorts { get; } = new Dictionary<string, IFlowPort>();
        public Dictionary<string, IFlowPort> FlowOutPorts { get; } = new Dictionary<string, IFlowPort>();
        
        private void DefineFlowPorts()
        {
            if (!UIXGraphReflector.NodeCache.Get(GetType(), out var data)) return;
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
        
        internal override void InternalDefinition()
        {
            base.InternalDefinition();
            DefineFlowPorts();
        }

        public void Initialize(ref IFlow flow)
        {
            try
            {
                InitializeValuePorts(ref flow);
                OnInitialize(ref flow);
            }
            catch
            {
                Debug.LogWarning($"Failed to Initialize Node {this} | {NodeId}");
                throw;
            }
        }

        private void InitializeValuePorts(ref IFlow flow)
        {
            foreach (var valueIn in ValueInPorts.Values)
            {
                valueIn.Initialize(ref flow);
            }

            foreach (var valueOut in ValueOutPorts.Values)
            {
                valueOut.Initialize(ref flow);
            }
        }
        
        protected virtual void OnInitialize(ref IFlow flow) {}
    }
}
