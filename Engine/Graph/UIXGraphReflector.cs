using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public class UIXNodeReflection : ITypeStorage
    {
        private static Type _isNodeAttribute = typeof(NodeAttribute);
        private static Vector2 _defaultSize = new Vector2(100, 300);
        
        public Type Type { get; set; }

        public IEnumerable<string> Path { get; set; }
        
        public string Name { get; set; }

        public string Help { get; set; }

        public bool Deletable { get; set; }

        public bool Moveable { get; set; }
        
        public Vector2 Size { get; set; }
        
        public HashSet<string> Tags { get; set; }
        
        public List<ValuePort> ValuePorts { get; set; }
        
        public List<FlowPort> FlowPorts { get; set; }
        
        private Dictionary<ContextMenu, MethodInfo> _contextMethods;
        
        public IReadOnlyDictionary<ContextMenu, MethodInfo> ContextMethods => _contextMethods;

        public bool ShouldCache(Type type)
        {
            var attr = type.GetCustomAttribute<NodeAttribute>();
            
            ExtractSettings(type, attr);
            ExtractValuePorts(type);
            ExtractFlowPorts(type);
            ExtractContextMethods(type);

            return true;
        }

        private void ExtractSettings(Type type, NodeAttribute attr)
        {
            bool isNull = attr == null;
            Type = type;
            Name = isNull || string.IsNullOrEmpty(attr.Name) ? type.Name.Replace("Node", "").Replace(".", "/") : attr.Name;
            Path = isNull || string.IsNullOrEmpty(attr.Path) ? type.Namespace?.Replace(".", "/").Split('/') : attr.Path.Split('/');
            Help = isNull ? "" : attr.Help;
            Tags = isNull ? new HashSet<string>() : new HashSet<string>(attr.Tags);
            Deletable = isNull || attr.Deletable;
            Moveable = isNull || attr.Moveable;
            Size = isNull ? _defaultSize : attr.Size;
        }

        private void ExtractValuePorts(Type type)
        {
            ValuePorts = new List<ValuePort>();
            // This OrderBy sorts the fields by subclasses first
            // we may want to put an "order" value or at least have InOut attributes process first so they are drawn first
            var infos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).OrderBy(field => field.MetadataToken);
            foreach (var info in infos)
            {
                var attrs = info.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case ValueInAttribute input:
                            ValuePorts.Add(new ValuePort(info, PortDirection.Input, input.Capacity, input.Name));
                            break;
                        case ValueOutAttribute output:
                            ValuePorts.Add(new ValuePort(info, PortDirection.Output, output.Capacity, output.Name));
                            break;
                        case ValueInOutAttribute inout:
                            ValuePorts.Add(new ValuePort(info, PortDirection.Input, inout.InCapacity, inout.InName));
                            ValuePorts.Add(new ValuePort(info, PortDirection.Output, inout.OutCapacity, inout.OutName));
                            break;
                    }
                }
            }
            // TODO: is this needed?
            //Ports.Sort((a,b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        private void ExtractFlowPorts(Type type)
        {
            FlowPorts = new List<FlowPort>();
            
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).OrderBy(field => field.MetadataToken);
            foreach (var methodInfo in methodInfos)
            {
                var attrs = methodInfo.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case FlowInAttribute input:
                            FlowPorts.Add(new FlowPort(methodInfo, input.Capacity, input.Name));
                            break;
                    }
                }
            }
            
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).OrderBy(field => field.MetadataToken);
            foreach (var fieldInfo in fieldInfos)
            {
                var attrs = fieldInfo.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case FlowOutAttribute input:
                            FlowPorts.Add(new FlowPort(fieldInfo, input.Capacity, input.Name));
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

    public class UIXGraphReflection : ITypeStorage
    {
        public HashSet<string> Tags { get; set; }
        
        public bool ShouldCache(Type type)
        {
            var attr = type.GetCustomAttribute<GraphAttribute>();
            
            ExtractSettings(type, attr);

            return true;
        }
        
        private void ExtractSettings(Type type, GraphAttribute attr)
        {
            bool isNull = attr == null;
            Tags = isNull ? new HashSet<string>() : new HashSet<string>(attr.Tags);
        }
    }

    public static class UIXGraphReflector
    {
        public static readonly TypeCache<INode, UIXNodeReflection> NodeCache = new TypeCache<INode, UIXNodeReflection>();
        
        public static readonly TypeCache<IGraph, UIXGraphReflection> GraphCache = new TypeCache<IGraph, UIXGraphReflection>();
    }
}