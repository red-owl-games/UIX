using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    {
        void Initialize();
        string NodeId { get; }
        string NodeTitle { get; }
        Rect NodeRect { get; set; }
        Vector2 NodePosition { get; set; }

        Dictionary<string, IValuePort> ValueInPorts { get; }
        Dictionary<string, IValuePort> ValueOutPorts { get; }
        Dictionary<string, IFlowPort> FlowInPorts { get; }
        Dictionary<string, IFlowPort> FlowOutPorts { get; }
    }

    [Serializable]
    public abstract class Node : INode 
    {
        private static Vector2 _defaultPosition = new Vector2(0, 0);

        [field: NonSerialized] // Prevents a weird editor domain reload bug where IsInitialized stays true
        protected bool IsInitialized { get; private set; }

#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private string nodeId;

        public string NodeId => nodeId;
        
#if ODIN_INSPECTOR 
        [HideInInspector]
#endif
        [SerializeField] 
        private string nodeTitle;

        public string NodeTitle => nodeTitle;

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

        private Dictionary<string, IValuePort> _valueInPorts;
        public Dictionary<string, IValuePort> ValueInPorts => _valueInPorts ?? (_valueInPorts = UIXGraphReflector.GetValuePorts(this, PortDirection.Input));
        
        private Dictionary<string, IValuePort> _valueOutPorts;
        public Dictionary<string, IValuePort> ValueOutPorts => _valueOutPorts ?? (_valueOutPorts = UIXGraphReflector.GetValuePorts(this, PortDirection.Output));

        private Dictionary<string, IFlowPort> _flowInPorts;
        public Dictionary<string, IFlowPort> FlowInPorts => _flowInPorts ?? (_flowInPorts = UIXGraphReflector.GetFlowPorts(this, PortDirection.Input));

        private Dictionary<string, IFlowPort> _flowOutPorts;
        public Dictionary<string, IFlowPort> FlowOutPorts => _flowOutPorts ?? (_flowOutPorts = UIXGraphReflector.GetFlowPorts(this, PortDirection.Output));

        protected Node()
        {
            nodeId = Guid.NewGuid().ToString();

            bool found = UIXGraphReflector.NodeCache.Get(GetType(), out var data);
            nodeTitle = found ? data.Name : "";
            nodeRect = found ? new Rect(_defaultPosition, data.Size) : new Rect(_defaultPosition, new Vector2(200, 100));
        }

        public void Initialize()
        {
            if (IsInitialized) return;
            try
            {
                // Hack to trigger initialization of ports
                _valueInPorts = ValueInPorts;
                _valueOutPorts = ValueOutPorts;
                _flowInPorts = FlowInPorts;
                _flowOutPorts = FlowOutPorts;
                
                OnInitialize();
                IsInitialized = true;
            }
            catch
            {
                IsInitialized = false;
                Debug.LogWarning($"Failed to Initialize Node {GetType().FullName} | {nodeId}");
            }
        }
        
        protected virtual void OnInitialize() {}
        
        public override string ToString() => $"{nodeTitle}";
    }
}
