using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    {
        string Id { get; set; }
        Vector2 Position { get; set; }
    }
    
    [Serializable]
    public abstract class Node : INode
    {
        [field: SerializeField]
        public string Id { get; set; }
        
        [field: SerializeField]
        public Vector2 Position { get; set; }
    }
}