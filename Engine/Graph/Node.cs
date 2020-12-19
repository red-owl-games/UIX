using System;
using System.Collections;
using System.Collections.Generic;
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
        Dictionary<string, ValuePort> ValuePorts { get; }
        Dictionary<string, FlowPort> FlowPorts { get; }
    }

    [Serializable]
    public abstract class Node : INode 
    {
        private static Vector2 _defaultPosition = new Vector2(0, 0);

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

        private Dictionary<string, ValuePort> _valuePorts;
        public Dictionary<string, ValuePort> ValuePorts => _valuePorts ?? (_valuePorts = Port.GetValuePorts(this));

        private Dictionary<string, FlowPort> _flowPorts;
        public Dictionary<string, FlowPort> FlowPorts => _flowPorts ?? (_flowPorts = Port.GetFlowPorts(this));

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
            OnInitialize();
            IsInitialized = true;
        }
        
        protected virtual void OnInitialize() {}
    }
}
