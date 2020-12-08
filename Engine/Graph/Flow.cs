using System.Collections;
using System.Collections.Generic;

namespace RedOwl.UIX.Engine
{
    public struct ControlPort
    {
    }
    
    public class Flow
    {
        private IGraph _graph;
        private Dictionary<string, object> _cache;

        public Flow(IGraph graph)
        {
            _graph = graph;
            _cache = new Dictionary<string, object>();
        }
        
        public T Get<T>(T store)
        {
            string key = "";
            if (_cache.ContainsKey(key))
            {
                return (T) _cache[key];
            }
            return default;
        }

        public void Set<T>(T store, object value)
        {
            string key = "";
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);
            }
        }
        
        public void Execute(StartNode start)
        {
            // foreach (var exit in GetFlowOutPorts(start))
            // {
            //     StartRoutine(start, exit);
            // }
            // For each ControlPort in 'start'
            //    get connected port
            //    
            //    recursion
            
            // IExecutableNode next = root;
            // int iterations = 0;
            // while (next != null)
            // {
            //     next = next.Execute(data);
            //
            //     iterations++;
            //     if (iterations > 2000)
            //     {
            //         Debug.LogError("Potential infinite loop detected. Stopping early.", this);
            //         break;
            //     }
            // }
        }

        // private UIXFlowPortReflection GetFlowInPort(INode node, string id)
        // {
        //     if (UIXGraphReflector.NodeCache.Get(node.GetType(), out var data))
        //     {
        //         foreach (var port in data.FlowPorts)
        //         {
        //             if (port.Direction == PortDirection.Input) continue;
        //             yield return port;
        //         }
        //     }
        //     
        // }
        //
        // private IEnumerable<UIXFlowPortReflection> GetFlowOutPorts(INode node)
        // {
        //     if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) yield break;
        //     foreach (var port in data.FlowPorts)
        //     {
        //         if (port.Direction == PortDirection.Input) continue;
        //         yield return port;
        //     }
        // }
        //
        // public IEnumerable Run(INode node, UIXFlowPortReflection port)
        // {
        //     if (GetConnection(node, port, out var connection))
        //     {
        //         if (_graph.GetNode(connection.Input.Node, out var other))
        //         {
        //             if (other.GetPort(connection.Input.Port, var)
        //         }
        //     }
        // }
        //
        // private bool GetConnection(INode node, UIXFlowPortReflection port, out Connection connection)
        // {
        //     foreach (var conn in _graph.Connections)
        //     {
        //         if (conn.Output.Node != node.Id) continue;
        //         if (conn.Output.Port != port.Hash) continue;
        //         //Found the right connection
        //         connection = conn;
        //         return true;
        //     }
        //
        //     connection = default;
        //     return false;
        // }
        //
        public void StartRoutine(INode startNode, ControlPort exit, float interval = 1f)
        {
            
        }

        // public void StartRoutine(INode start, UIXFlowPortReflection exit, float interval = 1f)
        // {
        //     
        // }
    }
}
