using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public interface INode
    {
        string Id { get; }
        Vector2 Position { get; set; }
    }
    
    [Serializable]
    public abstract class Node : INode
    {
        [field: HideInInspector]
        [field: SerializeField] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [field: HideInInspector]
        [field: SerializeField]
        public Vector2 Position { get; set; }
    }
}