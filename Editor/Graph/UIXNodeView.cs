using System;
using RedOwl.UIX.Engine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using PortView = UnityEditor.Experimental.GraphView.Port;

namespace RedOwl.UIX.Editor
{
    public class UIXNodeView : NodeView
    {
        /*
        private INode Model => (INode) userData;

        public UIXNodeView(INode model)
        {
            userData = model;
            Init();
        }

        public void Init()
        {
            title = NodeAttribute.GetTitle(Model.GetType());
            SetPosition(new Rect(Model.Position, new Vector2(100, 100)));
            CreateBody();
            CreatePorts();

            RefreshExpandedState();
            RefreshPorts();
            
            RegisterCallback<ChangeEvent<string>>(evt =>
            {
                Debug.Log($"Set Value {evt.previousValue} -> {evt.newValue} for {evt.target}");
            });
            RegisterCallback<ChangeEvent<int>>(evt =>
            {
                Debug.Log($"Set Value {evt.previousValue} -> {evt.newValue} for {evt.target}");
            });
            RegisterCallback<ChangeEvent<float>>(evt =>
            {
                Debug.Log($"Set Value {evt.previousValue} -> {evt.newValue} for {evt.target}");
            });
        }

        private void CreateBody()
        {
            UIXEditor.DrawInto(Model, extensionContainer);
        }

        private void CreatePorts()
        {
            Model.ForFieldWithType<INode, IPort>((info, field) =>
            {
                if (field is IValueInput)
                {
                    var p = PortView.Create<Edge>(field.Orientation, Direction.Input, field.Capacity, field.FieldType);
                    p.portName = field.Name;
                    inputContainer.Add(p);
                }

                if (field is IValueOutput)
                {
                    var p = PortView.Create<Edge>(field.Orientation, Direction.Output, field.Capacity, field.FieldType);
                    p.portName = field.Name;
                    outputContainer.Add(p);
                }
            });
        }
        */
    }
    
    public static class NodeViewExtensions
    {
        //public static INode INode(this NodeView self) => (INode) self.userData;
    }
}