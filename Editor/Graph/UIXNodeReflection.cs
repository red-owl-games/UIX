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
    public class UIXPortReflection
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

        public UIXPortReflection(FieldInfo field, Direction direction, PortView.Capacity capacity, string name = null)
        {
            Field = field;
            Name = name ?? field.Name;
            Type = field.FieldType;
            Direction = direction;
            Capacity = capacity;
            ShowElement = field.IsFamily;
            IsUsingFieldName = name != null;
        }
    }

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
        
        public List<string> Tags { get; set; }
        
        public List<UIXPortReflection> Ports { get; set; }
        
        private Dictionary<ContextMenu, MethodInfo> _contextMethods;
        
        public IReadOnlyDictionary<ContextMenu, MethodInfo> ContextMethods => _contextMethods;

        public bool ShouldCache(Type type)
        {
            var attr = type.GetCustomAttribute<NodeAttribute>();
            
            ExtractSettings(type, attr);
            ExtractTags(type);
            ExtractPorts(type);
            ExtractContextMethods(type);

            return true;
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
            Size = isNull ? _defaultSize : attr.Size;
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
            Ports = new List<UIXPortReflection>();
            // This OrderBy sorts the fields by subclasses first
            // we may want to put an "order" value or at least have InOut attributes process first so they are drawn first
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(field => field.MetadataToken);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    switch (attr)
                    {
                        case InputAttribute input:
                            Ports.Add(new UIXPortReflection(field, Direction.Input, input.Capacity, input.Name));
                            break;
                        case OutputAttribute output:
                            Ports.Add(new UIXPortReflection(field, Direction.Output, output.Capacity, output.Name));
                            break;
                        case InOutAttribute inout:
                            Ports.Add(new UIXPortReflection(field, Direction.Input, inout.InCapacity, inout.InName));
                            Ports.Add(new UIXPortReflection(field, Direction.Output, inout.OutCapacity, inout.OutName));
                            break;
                    }
                }
            }
            // TODO: is this needed?
            //Ports.Sort((a,b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
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

    public static partial class UIXReflector
    {
        public static readonly TypeCache<Node, UIXNodeReflection> NodeCache = new TypeCache<Node, UIXNodeReflection>();

        // [MenuItem("Project/Rebuild Cache")]
        // private static void RebuildCache()
        // {
        //     NodeCache.ShouldBuildCache();
        // }
    }
}