using System;
using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IFlowPort : IPort
    {
        IEnumerator Execute(IFlow flow);
    }
    
    [Serializable]
    public class FlowPort : Port<FlowPortSettings>, IFlowPort
    {
        private static Type _voidType = typeof(void);
        private static Type _flowType = typeof(IFlow);
        private static Type _flowPortType = typeof(IFlowPort);
        private static Type _enumeratorType = typeof(IEnumerator);
        private static Type _simpleCallbackType = typeof(Action<IFlow>);
        private static Type _syncCallbackType = typeof(Func<IFlow, IFlowPort>);
        private static Type _asyncCallbackType = typeof(Func<IFlow, IEnumerator>);
        
        public FlowPort(INode node) : base(node) {}
        public FlowPort(IFlowPort port) : base(port) {}

        private bool _hasCallback;
        private Action<IFlow> _simpleCallback;
        private Func<IFlow, IFlowPort> _syncCallback;
        private Func<IFlow, IEnumerator> _asyncCallback;

        public override void Initialize<TNode>(TNode node, UIXNodeReflection nodeData , FlowPortSettings portData)
        {
            Name = portData.Name;
            Direction = portData.Direction;
            Capacity = portData.Capacity;
            if (portData.Callback == null) return;
            var parameters = portData.Callback.GetParameters();
            if (parameters.Length != 1)
            {
                Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}Node.{portData.Callback.Name}' has {parameters.Length} parameter(s).  Can only accept 1 parameter of type '{_flowType}'");
                return;
            }

            var paramType = parameters[0].ParameterType;
            if (!typeof(IFlow).IsAssignableFrom(paramType))
            {
                Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}Node.{portData.Callback.Name}' has 1 parameter that takes type '{paramType}'.  Can only accept 1 parameter of type '{_flowType}'");
                return;
            }
            _hasCallback = false;
            if (_voidType.IsAssignableFrom(portData.Callback.ReturnType))
            {
                _simpleCallback = (Action<IFlow>)portData.Callback.CreateDelegate(_simpleCallbackType, node);
                _hasCallback = true;
            }
            if (_flowPortType.IsAssignableFrom(portData.Callback.ReturnType))
            {
                _syncCallback = (Func<IFlow, IFlowPort>)portData.Callback.CreateDelegate(_syncCallbackType, node);
                _hasCallback = true;
            }
            if (_enumeratorType.IsAssignableFrom(portData.Callback.ReturnType))
            {
                _asyncCallback = (Func<IFlow, IEnumerator>)portData.Callback.CreateDelegate(_asyncCallbackType, node);
                _hasCallback = true;
            }
            if (!_hasCallback) Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}Node.{portData.Callback.Name}' did not have one of the following method signatures [Action<IFlow>, Func<IFlow, IFlowPort>, Func<IFlow, IEnumerator>]");
            // TODO: Log about bad callback setup?
        }

        public IEnumerator Execute(IFlow flow)
        {
            if (_hasCallback)
            {
                Debug.Log($"Entered Flowport Execute Function");
                if (_simpleCallback != null)
                {
                    
                }

                if (_syncCallback != null)
                {
                    
                }

                if (_asyncCallback != null)
                {
                    
                }
            }
            
            yield break;
        }

        private static Type _valueType = typeof(FlowPort);
        public Type ValueType => _valueType;
    }
}