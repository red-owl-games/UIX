using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface IGraph
    {
        IEnumerable<T> GetNodes<T>() where T : INode;
        bool GetNode(string id, out INode node);
        IEnumerable<INode> Nodes { get; }
        void AddNode(INode node);
        void RemoveNode(INode node);
        
        IEnumerable<Connection> Connections { get; }
        void AddConnection(Connection connection);
        void RemoveConnection(Connection connection);
    }

    [Node(Path = "RedOwl")]
    [Serializable]
    public abstract class Graph : ScriptableObject, IGraph //,INode
    {
        [field: HideInInspector]
        [field: SerializeField] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [field: HideInInspector]
        [field: SerializeField]
        public Vector2 Position { get; set; }
        
        [SerializeReference] 
        private List<INode> _nodes;
        public IEnumerable<INode> Nodes => _nodes;
        
        [SerializeField]
        private List<Connection> _connections;
        public IEnumerable<Connection> Connections => _connections;
        
        protected Graph()
        {
            _nodes = new List<INode>();
            _connections = new List<Connection>();
        }

        public IEnumerable<T> GetNodes<T>() where T : INode
        {
            var type = typeof(T);
            foreach (var node in _nodes)
            {
                if (type.IsInstanceOfType(node)) yield return (T) node;
            }
        }

        public bool GetNode(string id, out INode node)
        {
            foreach (var n in _nodes)
            {
                if (n.Id != id) continue;
                node = n;    
                return true;
            }

            node = null;
            return false;
        }
        
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

        public void RemoveConnection(Connection connection)
        {
            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                if (_connections[i].Id == connection.Id) _connections.RemoveAt(i);
            }
        }
    }
}