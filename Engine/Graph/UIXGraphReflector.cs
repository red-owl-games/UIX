using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    public class UIXValuePortReflection : IPortReflectionData
    {
        public FieldInfo Info { get; }

        public string Name { get; }

        public PortDirection Direction { get; }

        public PortCapacity Capacity { get; }

        public bool ShowElement { get; }


        public UIXValuePortReflection(FieldInfo info, PortDirection direction, PortCapacity capacity, string name = null)
        {
            Info = info;
            Name = name ?? info.Name;
            Direction = direction;
            Capacity = capacity;
            ShowElement = info.IsFamily;
            //IsUsingFieldName = name != null; ???
        }
        
        public string PortId(INode node)
        {
            return Info.GetValue(node) is Port port ? port.PortId : "";
        }
    }
    
     public class UIXFlowPortReflection : IPortReflectionData
     {
         private static readonly Type EnumerableType = typeof(IEnumerable);
         
         public FieldInfo Info { get; }

         public string Name { get; }

         public PortDirection Direction { get; }
         
         public PortCapacity Capacity { get; }
         
         public bool IsAsync { get; }

         public UIXFlowPortReflection(FieldInfo info, PortDirection direction, PortCapacity capacity, string name = null)
         {
             Info = info;
             Name = name ?? info.Name;
             Direction = direction;
             Capacity = capacity;
             IsAsync = false;
         }
         
         public string PortId(INode node)
         {
             return Info.GetValue(node) is Port port ? port.PortId : "";
         }
     }

    public class UIXNodeReflection : ITypeStorage
    {
        private static BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private static Type _isNodeAttribute = typeof(NodeAttribute);
        private static Vector2 _defaultSize = new Vector2(100, 300);
        
        public Type Type { get; set; }

        public IEnumerable<string> Path { get; set; }
        
        public string Name { get; set; }

        public string Help { get; set; }

        public bool Deletable { get; set; }

        public bool Moveable { get; set; }
        
        public Vector2 Size { get; set; }
        
        public bool IsRootNode { get; set; }
        
        public HashSet<string> Tags { get; set; }
        
        public List<UIXValuePortReflection> ValuePorts { get; set; }
        
        public List<UIXFlowPortReflection> FlowPorts { get; set; }
        
        private Dictionary<string, MethodInfo> _methods;
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
            Help = isNull ? "" : attr.Tooltip;
            Tags = isNull ? new HashSet<string>() : new HashSet<string>(attr.Tags);
            Deletable = isNull || attr.Deletable;
            Moveable = isNull || attr.Moveable;
            IsRootNode = isNull ? false : attr.IsRootNode;
            Size = isNull ? _defaultSize : attr.Size;
        }

        private void ExtractValuePorts(Type type)
        {
            ValuePorts = new List<UIXValuePortReflection>();
            // This OrderBy sorts the fields by subclasses first
            // we may want to put an "order" value or at least have InOut attributes process first so they are drawn first
            var infos = type.GetFields(_bindingFlags).OrderBy(field => field.MetadataToken);
            foreach (var info in infos)
            {
                var attrs = info.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case ValueInAttribute input:
                            ValuePorts.Add(new UIXValuePortReflection(info, PortDirection.Input, input.Capacity, input.Name));
                            break;
                        case ValueOutAttribute output:
                            ValuePorts.Add(new UIXValuePortReflection(info, PortDirection.Output, output.Capacity, output.Name));
                            break;
                    }
                }
            }
            // TODO: is this needed?
            //Ports.Sort((a,b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        private void ExtractFlowPorts(Type type)
        {
            FlowPorts = new List<UIXFlowPortReflection>();
            var fieldInfos = type.GetFields(_bindingFlags).OrderBy(field => field.MetadataToken);
            foreach (var fieldInfo in fieldInfos)
            {
                var attrs = fieldInfo.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case FlowInAttribute input:
                            // if (input.Callback != null)
                            // {
                            //     var info = type.GetMethod(input.Callback, _bindingFlags);
                            //     //if (info != null && info.GetParameters().Length == 1) Debug.Log($"Input Has Callback - {info.ReturnType}");
                            // }
                            FlowPorts.Add(new UIXFlowPortReflection(fieldInfo, PortDirection.Input, input.Capacity, input.Name));
                            break;
                        case FlowOutAttribute output:
                            // if (output.Callback != null)
                            // {
                            //     var info = type.GetMethod(output.Callback, _bindingFlags);
                            //     //if (info != null && info.GetParameters().Length == 1) Debug.Log($"Output Has Callback - {info.ReturnType}");
                            // }
                            FlowPorts.Add(new UIXFlowPortReflection(fieldInfo, PortDirection.Output, output.Capacity, output.Name));
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
        
        public static Dictionary<string, IFlowPort> GetFlowPorts<T>(T node, PortDirection direction) where T : INode
        {
            if (!NodeCache.Get(node.GetType(), out var data)) return new Dictionary<string, IFlowPort>();
            var output = new Dictionary<string, IFlowPort>(data.FlowPorts.Count);
            foreach (var port in data.FlowPorts)
            {
                if (port.Direction != direction) continue;
                // TODO: These need to be "Proxy" objects composed of the data rather then the real ones.
                var newPort = new FlowPort((FlowPort) port.Info.GetValue(node));
                newPort.Initialize(node, data, port);
                output.Add(port.PortId(node), newPort); 
            }

            return output;
        }

        public static Dictionary<string, IValuePort> GetValuePorts<T>(T node, PortDirection direction) where T : INode
        {
            if (!NodeCache.Get(node.GetType(), out var data)) return new Dictionary<string, IValuePort>();
            var output = new Dictionary<string, IValuePort>(data.ValuePorts.Count);
            foreach (var port in data.ValuePorts)
            {
                if (port.Direction != direction) continue;
                // TODO: These need to be "Proxy" objects composed of the data rather then the real ones.
                var newPort = new ValuePort((ValuePort) port.Info.GetValue(node));
                newPort.Initialize(node, data, port);
                output.Add(port.PortId(node), newPort);
            }

            return output;
        }
    }
}