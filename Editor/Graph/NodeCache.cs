using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = RedOwl.UIX.Engine.Node;
using PortView = UnityEditor.Experimental.GraphView.Port;

namespace RedOwl.UIX.Editor
{
    public class PortReflection
    {
        public FieldInfo Field { get; set; }
        
        public string Name { get; set; }

        public Type Type { get; set; }

        public Direction Direction { get; set; }

        public PortView.Capacity Capacity { get; set; }

        public bool ShowElement { get; set; }

        /// <summary>
        /// Is this.name just the this.field or set via the attribute
        /// </summary>
        public bool IsUsingFieldName { get; set; }

        public PortReflection(FieldInfo field, InputAttribute attr, Direction direction)
        {
            Field = field;
            Name = attr.Name ?? field.Name;
            Type = field.FieldType;
            Direction = direction;
            Capacity = attr.Capacity;
            ShowElement = field.IsFamily;
            IsUsingFieldName = attr.Name != null;
        }

        public PortReflection(FieldInfo field, OutputAttribute attr, Direction direction)
        {
            Field = field;
            Name = attr.Name ?? field.Name;
            Type = field.FieldType;
            Direction = direction;
            Capacity = attr.Capacity;
            // TODO: And has corresponding "INPUT" ?
            ShowElement = false;
            IsUsingFieldName = attr.Name != null;
        }
    }

    public class NodeReflection : ITypeStorage
    {
        private static Type _isNodeAttribute = typeof(NodeAttribute);
        
        public Type Type { get; set; }

        public IEnumerable<string> Path { get; set; }
        
        public string Name { get; set; }

        public string Help { get; set; }

        public bool Deletable { get; set; }

        public bool Moveable { get; set; }
        
        public List<string> Tags { get; set; }
        
        public List<PortReflection> Ports { get; set; }
        
        private Dictionary<ContextMenu, MethodInfo> _contextMethods;
        
        public IReadOnlyDictionary<ContextMenu, MethodInfo> ContextMethods => _contextMethods;

        public bool ShouldCache(Type type)
        {
            var attr = type.GetCustomAttribute<NodeAttribute>();
            
            ExtractSettings(type, attr);
            ExtractTags(type);
            ExtractPorts(type);
            ExtractContextMethods(type);

            Debug.Log($"Caching Node '{Name}' '{string.Join("/", Path)}' | {Deletable} {Moveable}");
            return true;
        }
        
        private void ExtractSettings(Type type)
        {
            Type = type;
            Name = ObjectNames.NicifyVariableName(type.Name.Replace("Node", ""));
            Path = new[]{ObjectNames.NicifyVariableName(type.Namespace)};
            Help = "";
            Deletable = true;
            Moveable = true;
        }

        private void ExtractSettings(Type type, NodeAttribute attr)
        {
            bool isNull = attr == null;
            Type = type;
            Name = isNull || string.IsNullOrEmpty(attr.Name) ? ObjectNames.NicifyVariableName(type.Name.Replace("Node", "").Replace(".", "/")) : attr.Name;
            Path = isNull || string.IsNullOrEmpty(attr.Path) ? ObjectNames.NicifyVariableName(type.Namespace?.Replace(".", "/")).Split('/') : attr.Path.Split('/');
            Help = isNull ? "" : attr.Help;
            Deletable = isNull || attr.Deletable;
            Moveable = isNull || attr.Moveable;
        }
        
        private void ExtractTags(Type type)
        {
            Tags = new List<string>();
            var tagsAttr = type.GetCustomAttribute<TagsAttribute>();
            if (tagsAttr != null)
            {
                Tags.AddRange(tagsAttr.Tags);
            }
        }

        private void ExtractPorts(Type type)
        {
            Ports = new List<PortReflection>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case InputAttribute input:
                            Ports.Add(new PortReflection(field, input, Direction.Input));
                            break;
                        case OutputAttribute output:
                            Ports.Add(new PortReflection(field, output, Direction.Output));
                            break;
                    }
                }
            }
        }
        
        private void ExtractContextMethods(Type type)
        {
            _contextMethods = new Dictionary<ContextMenu, MethodInfo>();
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var contextAttr = method.GetCustomAttribute<ContextMenu>();
                if (contextAttr != null)
                {
                    _contextMethods.Add(contextAttr, method);
                }
            }
        }


    }

    public static partial class Reflector
    {
        private static readonly TypeCache<Node, NodeReflection> NodeCache = new TypeCache<Node, NodeReflection>();

        [MenuItem("Project/Rebuild Cache")]
        private static void RebuildCache()
        {
            NodeCache.ShouldBuildCache();
        }
    }
}