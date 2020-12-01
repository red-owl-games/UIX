using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedOwl.UIX.Engine
{
    public interface IGraph
    {
        /*
        IEnumerable<INode> Nodes { get; }
        IEnumerable<Connection> Connections { get; }
        void AddNode(INode node);
        void RemoveNode(INode node);
        void AddConnection(Connection connection);
        void RemoveConnection(string output, string input);
        */
    }

    [Serializable]
    public abstract class Graph<T> : Node, IGraph where T : INode
    {
        /*
        [SerializeReference] private List<INode> _nodes;

        [SerializeField] private List<Connection> _connections;
        
        public IEnumerable<INode> Nodes => _nodes;
        public IEnumerable<Connection> Connections => _connections;
        public void AddNode(INode node) => _nodes.Add(node);

        public void RemoveNode(INode node)
        {
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                var n = _nodes[i];
                if (n.Id == node.Id) _nodes.RemoveAt(i);
            }
        }
        
        public void AddConnection(Connection connection) => _connections.Add(connection);

        public void RemoveConnection(string output, string input)
        {
            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                var conn = _connections[i];
                if (conn.Output == output && conn.Input == input) _connections.RemoveAt(i);
            }
        }

        protected Graph()
        {
            _nodes = new List<INode>();
            _connections = new List<Connection>();
            _nodeCache = new NodeCache<T>();
        }
        */
    }
    
    public class NullGraph : Graph<INode>, IGraph
    {
    }
}