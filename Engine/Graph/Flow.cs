using System;
using System.Linq;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IFlow
    {
        IGraph Graph { get; }
        IFlowNode[] RootNodes { get; }
        T Get<T>(IPort port);
        bool Get<T>(IPort port, out T output);
        void Set(IPort port, object value);

        void Execute();
    }

    public class Flow : BetterDictionary<PortId, object>, IFlow
    {
        public IGraph Graph { get; }
        public IFlowNode[] RootNodes { get; }
        
        public Flow(IGraph graph, IFlowNode[] nodes = null)
        {
            Graph = graph;
            RootNodes = nodes ?? graph.RootNodes.ToArray();
        }
        
        public T Get<T>(IPort port)
        {
            // TODO: use Converter.Convert ?
            if (TryGetValue(port.Id, out var item))
            {
                try
                {
                    return (T)item;
                }
                catch(InvalidCastException)
                {
                    Debug.Log($"Unable To Cast to {typeof(T)}");
                    return default;
                }
            }
            return default;
        }

        public bool Get<T>(IPort port, out T output)
        {
            // TODO: use Converter.Convert ?
            if (TryGetValue(port.Id, out var item))
            {
                output = (T) item;
                return true;
            }
            output = default;
            return false;
        }

        public void Set(IPort port, object value)
        {
            // TODO: use Converter.Convert ?
            if (ContainsKey(port.Id))
            {
                this[port.Id] = value;
            }
            else
            {
                Add(port.Id, value);
            }
        }
        
        public void Execute()
        {
            Graph.Initialize();
            Clear();
            // TODO: Remove Timer
            using(new Timer("Flow.Execution"))
            {
                foreach (var node in RootNodes)
                {
                    //Debug.Log($"Starting Root Node Walk from '[{node}]'");
                    WalkFlowPorts(this, node);
                }
            }
        }

        // public static void ExecutePort(IFlow flow, IValuePort port)
        // {
        //     // TODO: This might need to be a custom while loop executor to handle supporting Yield Instructions
        //     while (port.Execute(flow).MoveNext()) {}
        // }
        
        public static void ExecutePort(IFlow flow, IFlowPort port)
        {
            // TODO: This might need to be a custom while loop executor to handle supporting Yield Instructions
            while (port.Execute(flow).MoveNext()) {}
        }
        
        public static void WalkValuePorts(IFlow flow, INode node)
        {
            // Should only ever be called on "Nodes" that are about to have its FlowIn port executed
            // Recursively walks back up the value input ports connections to make sure values are "set" into the flow
            foreach (var port in node.ValueInPorts.Values)
            {
                WalkValueIn(flow, node, port);
            }
        }

        public static void WalkValueIn(IFlow flow, INode node, IValuePort input)
        {
            // Debug.Log($"Walking ValueIn '[{node}]{input}'");
            // ExecutePort(flow, input);
            foreach (var connection in flow.Graph.ValueInConnections.SafeGet(input.Id))
            {
                var nextNode = flow.Graph.GetNode(connection.Node); //  TODO: Needs saftey check?
                var output = nextNode.ValueOutPorts[connection.Port]; // TODO: Needs saftey check?
                // Debug.Log($"Pulled Value for '[{node}]{input}' from '[{nextNode}]{output}'");
                output.Set(flow, input);
            }
        }

        public static void WalkFlowPorts(IFlow flow, IFlowNode node)
        {
            // Should only ever be called on "Root Nodes" or nodes that "fork" the flow
            // This will walk recursive down the nodes flow out ports
            foreach (var port in node.FlowOutPorts.Values)
            {
                WalkFlowOut(flow, node, port);
            }
        }

        public static void WalkFlowOut(IFlow flow, IFlowNode node, IFlowPort output)
        {
            //Debug.Log($"Walking FlowOut '[{node}]{output}'");
            if (node.IsFlowRoot) WalkValuePorts(flow, node);
            ExecutePort(flow, output);
            foreach (var input in flow.Graph.FlowOutConnections.SafeGet(output.Id))
            {
                var nextNode = (IFlowNode)flow.Graph.GetNode(input.Node); //  TODO: Needs saftey check?
                var nextPort = nextNode.FlowInPorts[input.Port]; // TODO: Needs saftey check?
                //Debug.Log($"Traversing Towards '[{nextNode}]{nextPort}'");
                WalkFlowIn(flow, nextNode, nextPort);
            }
        }

        public static void WalkFlowIn(IFlow flow, IFlowNode node, IFlowPort input)
        {
            //Debug.Log($"Walking FlowIn '[{node}]{input}'");
            WalkValuePorts(flow, node);
            var enumerator = input.Execute(flow);
            while (enumerator.MoveNext())
            {
                // TODO: Handle Yield Instructions / Custom Yield Instructions
                if (enumerator.Current is IFlowPort nextPort) // TODO: saftey check for being given a FlowOut port?
                {
                    //Debug.Log($"Traversing Towards '[{node}]{nextPort}'");
                    WalkFlowOut(flow, node, nextPort);
                }
            }
        }
    }

    public class Flow<T> : Flow where T : IFlowNode
    {
        public Flow(IGraph graph) : base(graph, graph.GetNodes(typeof(T)).Cast<IFlowNode>().ToArray()) {}
    }
    
    // TODO: AsyncFlow<T> ?
}
