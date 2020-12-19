using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IFlow
    {
        T Get<T>(Port port);
        bool Get<T>(Port port, out T output);

        void Set<T>(Port port, T value);
    }

    public class Flow : IFlow
    {
        private readonly BetterDictionary<PortId, object> _cache;
        private IGraph _graph;
        protected List<INode> _startNodes;

        public Flow(IGraph graph, params INode[] startNodes)
        {
            _cache = new BetterDictionary<PortId, object>();
            _graph = graph;
            _graph.Initialize();
            _startNodes = new List<INode>(startNodes);
        }

        public T Get<T>(Port port)
        {
            // TODO: use Converter.Convert ?
            if (_cache.TryGetValue(port.Id, out var item))
            {
                return (T)item;
            }
            return default;
        }

        public bool Get<T>(Port port, out T output)
        {
            // TODO: use Converter.Convert ?
            if (_cache.TryGetValue(port.Id, out var item))
            {
                output = (T) item;
                return true;
            }
            output = default;
            return false;
        }

        public void Set<T>(Port port, T value)
        {
            // TODO: use Converter.Convert ?
            if (_cache.ContainsKey(port.Id))
            {
                _cache[port.Id] = value;
            }
            else
            {
                _cache.Add(port.Id, value);
            }
        }

        public void Execute()
        {
            _cache.Clear();
            foreach (var node in _startNodes)
            {
                foreach (var output in node.FlowPorts.Values)
                {
                    // Check if Is Output otherwise skip?
                    var enumerator = WalkOutput(output);
                    while (enumerator.MoveNext()) {}
                }
            }
        }

        private IEnumerator WalkOutput(Port output)
        {
            while (output.Execute().MoveNext()) {}
            foreach (var input in output.Connections)
            {
                yield return WalkInput(_graph.GetFlowPort(input));
            }
        }

        private IEnumerator WalkInput(Port port)
        {
            var enumerator = port.Execute();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is FlowPort output)
                {
                    yield return WalkOutput(output);
                }
            }
        }

        //
        // public IEnumerator ExecuteAsync()
        // {
        //     _cache.Clear();
        //     foreach (var node in _startNodes)
        //     {
        //         var enumerator = Run(node);
        //         while (enumerator.MoveNext())
        //         {
        //             yield return enumerator.Current;
        //         }
        //     }
        //     yield break;
        // }
        //
        // private IEnumerator Run(INode node)
        // {
        //     Debug.Log($"Executing Graph @ {node.GetType().FullName}|{node.Id}");
        //     foreach (var connection in _graph.FlowConnections)
        //     {
        //         if (connection.Key.Node == node.Id)
        //         {
        //             yield return HandleFlowOutPort(connection.Key);
        //         }
        //     }
        // }
        //
        // private IEnumerator HandleFlowOutPort(PortId id)
        // {
        //     if (!_graph.Get(id.Node, out var node)) yield break;
        //     
        //     var port = node.GetFlowPort(id.Port);
        //     var enumerator = ExecutePort(port).GetEnumerator();
        //     while (enumerator.MoveNext())
        //     {
        //         yield return enumerator.Current;
        //     }
        //     foreach (var connected in _graph.FlowConnections[id])
        //     {
        //         yield return HandleFlowInPort(connected);
        //     }
        // }
        //
        // private IEnumerator HandleFlowInPort(PortId id)
        // {
        //     if (!_graph.Get(id.Node, out var node)) yield break;
        //     // Walk Nodes Value Ports to ensure data is present upon FlowInPort execute
        //     var port = node.GetFlowPort(id.Port);
        //     var enumerator = ExecutePort(port).GetEnumerator();
        //     while (enumerator.MoveNext())
        //     {
        //         yield return HandleFlowOutPort(enumerator.Current);
        //     }
        // }
        //
        // private IEnumerable<FlowPort> ExecutePort(FlowPort port)
        // {
        //     Debug.Log($"Executing Port @ {port}");
        //     yield break;
        // }
    }

    public class Flow<T> : Flow where T : INode
    {
        public Flow(IGraph graph) : base(graph)
        {
            _startNodes = new List<T>(graph.GetNodes<T>()).ConvertAll(x => (INode)x);
        }
    }
}
//     
//     public class Flow
//     {

//         
//         public void Execute(StartNode start)
//         {
//             // foreach (var exit in GetFlowOutPorts(start))
//             // {
//             //     StartRoutine(start, exit);
//             // }
//             // For each ControlPort in 'start'
//             //    get connected port
//             //    
//             //    recursion
//             
//             // IExecutableNode next = root;
//             // int iterations = 0;
//             // while (next != null)
//             // {
//             //     next = next.Execute(data);
//             //
//             //     iterations++;
//             //     if (iterations > 2000)
//             //     {
//             //         Debug.LogError("Potential infinite loop detected. Stopping early.", this);
//             //         break;
//             //     }
//             // }
//         }
//
//         // private UIXFlowPortReflection GetFlowInPort(INode node, string id)
//         // {
//         //     if (UIXGraphReflector.NodeCache.Get(node.GetType(), out var data))
//         //     {
//         //         foreach (var port in data.FlowPorts)
//         //         {
//         //             if (port.Direction == PortDirection.Input) continue;
//         //             yield return port;
//         //         }
//         //     }
//         //     
//         // }
//         //
//         // private IEnumerable<UIXFlowPortReflection> GetFlowOutPorts(INode node)
//         // {
//         //     if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) yield break;
//         //     foreach (var port in data.FlowPorts)
//         //     {
//         //         if (port.Direction == PortDirection.Input) continue;
//         //         yield return port;
//         //     }
//         // }
//         //
//         // public IEnumerable Run(INode node, UIXFlowPortReflection port)
//         // {
//         //     if (GetConnection(node, port, out var connection))
//         //     {
//         //         if (_graph.GetNode(connection.Input.Node, out var other))
//         //         {
//         //             if (other.GetPort(connection.Input.Port, var)
//         //         }
//         //     }
//         // }
//         //
//         // private bool GetConnection(INode node, UIXFlowPortReflection port, out Connection connection)
//         // {
//         //     foreach (var conn in _graph.Connections)
//         //     {
//         //         if (conn.Output.Node != node.Id) continue;
//         //         if (conn.Output.Port != port.Hash) continue;
//         //         //Found the right connection
//         //         connection = conn;
//         //         return true;
//         //     }
//         //
//         //     connection = default;
//         //     return false;
//         // }
//         //
//         public void StartRoutine(INode startNode, ControlPort exit, float interval = 1f)
//         {
//             
//         }
//
//         // public void StartRoutine(INode start, UIXFlowPortReflection exit, float interval = 1f)
//         // {
//         //     
//         // }
//     }
// }
