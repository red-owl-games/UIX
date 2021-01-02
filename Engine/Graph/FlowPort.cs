using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace RedOwl.UIX.Engine
{
    public interface IFlowPort : IPort
    {
        void Definition(INode node, FlowPortSettings settings);
        IEnumerator Execute(IFlow flow);
    }
    
    [Preserve]
    public class FlowPort : Port, IFlowPort
    {
        private static Type _voidType = typeof(void);
        private static Type _flowType = typeof(IFlow);
        private static Type _flowPortType = typeof(IFlowPort);
        private static Type _enumeratorType = typeof(IEnumerator);
        private static Type _simpleCallbackType = typeof(Action<IFlow>);
        private static Type _syncCallbackType = typeof(Func<IFlow, IFlowPort>);
        private static Type _asyncCallbackType = typeof(Func<IFlow, IEnumerator>);
        
        public Type ValueType => _flowPortType;

        private bool _hasCallback;
        private Action<IFlow> _simpleCallback;
        private Func<IFlow, IFlowPort> _syncCallback;
        private Func<IFlow, IEnumerator> _asyncCallback;
        
        [Preserve]
        public FlowPort() {}

        public void Definition(INode node, FlowPortSettings settings)
        {
            Id = new PortId(node.NodeId, settings.Name);
            Name = settings.Name;
            Direction = settings.Direction;
            Capacity = settings.Capacity;
            if (settings.Callback == null) return;
            var parameters = settings.Callback.GetParameters();
            if (parameters.Length != 1)
            {
                Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}.{settings.Callback.Name}' has {parameters.Length} parameter(s).  Can only accept 1 parameter of type 'IFlow'");
                return;
            }

            var paramType = parameters[0].ParameterType;
            if (paramType != _flowType)
            {
                Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}.{settings.Callback.Name}' has 1 parameter that takes type '{paramType}'.  Can only accept 1 parameter of type 'IFlow'");
                return;
            }
            _hasCallback = false;
            if (_voidType.IsAssignableFrom(settings.Callback.ReturnType))
            {
                _simpleCallback = (Action<IFlow>)settings.Callback.CreateDelegate(_simpleCallbackType, node);
                _hasCallback = true;
            }
            if (_flowPortType.IsAssignableFrom(settings.Callback.ReturnType))
            {
                _syncCallback = (Func<IFlow, IFlowPort>)settings.Callback.CreateDelegate(_syncCallbackType, node);
                _hasCallback = true;
            }
            if (_enumeratorType.IsAssignableFrom(settings.Callback.ReturnType))
            {
                _asyncCallback = (Func<IFlow, IEnumerator>)settings.Callback.CreateDelegate(_asyncCallbackType, node);
                _hasCallback = true;
            }
            if (!_hasCallback) Debug.LogWarning($"FlowPort Callback for '{node.NodeTitle}.{settings.Callback.Name}' did not have one of the following method signatures [Action<IFlow>, Func<IFlow, IFlowPort>, Func<IFlow, IEnumerator>]");
            // TODO: Log about bad callback setup?
        }

        public IEnumerator Execute(IFlow flow)
        {
            if (_hasCallback)
            {
                _simpleCallback?.Invoke(flow);

                if (_syncCallback != null)
                {
                    yield return _syncCallback(flow);
                }

                if (_asyncCallback != null)
                {
                    var enumerator = _asyncCallback(flow);
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }
    }
}