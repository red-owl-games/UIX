using System;
using UnityEditor;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeAttribute : Attribute
    {
        /// <summary>
        /// Display name of the node.
        ///
        /// If not supplied, this will be inferred based on the class name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tooltip help content displayed for the node.
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// Slash-delimited path to categorize this node in the search window.
        /// </summary>
        public string Path { get; set; }
        
        public bool Deletable { get; set; } = true;
        
        public bool Moveable { get; set; } = true;
        
        public NodeAttribute(string path = null, string name = null)
        {
            Name = name;
            Path = path;
        }

        // public static string GetContextPath(Type nodeType)
        // {
        //     return nodeType.TryGetAttr<NodeAttribute>(out var attr)
        //         ? attr._contextMenuPath
        //         : $"{ObjectNames.NicifyVariableName(nodeType.Namespace)}/{ObjectNames.NicifyVariableName(nodeType.Name).Replace("Node", "")}";
        // }
        //
        // public static string GetTitle(Type nodeType)
        // {
        //     return nodeType.TryGetAttr<NodeAttribute>(out var attr)
        //         ? attr._title
        //         : ObjectNames.NicifyVariableName(nodeType.Name).Replace("Node", "");
        // }
    }
}