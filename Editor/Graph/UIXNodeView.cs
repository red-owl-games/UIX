using System;
using RedOwl.UIX.Engine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using PortView = UnityEditor.Experimental.GraphView.Port;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace RedOwl.UIX.Editor
{
    public class UIXNodeView : NodeView
    {
        private INode Model => (INode) userData;

        public UIXNodeView(INode node, UIXNodeReflection data)
        {
            userData = node;
            Initialize(data);
        }

        private void Initialize(UIXNodeReflection data)
        {
            name = Model.Id;
            title = data.Name;
            SetPosition(new Rect(Model.Position, data.Size));

            CreateBody();
            CreateFlowPorts(data);
            CreateValuePorts(data);
            RefreshExpandedState();
            RefreshPorts();
        }
        
        private void CreateBody()
        {
            UIXEditor.DrawInto(Model, extensionContainer);
        }
        
        private void CreateValuePorts(UIXNodeReflection data)
        {
            // TODO: a Custom UIXPortView might be needed to support things
            foreach (var portData in data.ValuePorts)
            {
                var port = PortView.Create<Edge>(Orientation.Horizontal, ConvertDirection(portData.Direction), ConvertCapacity(portData.Capacity), portData.Type);
                port.name = portData.Hash.ToString();
                port.userData = portData.Hash;
                port.portName = portData.Name;
                (portData.Direction == PortDirection.Input ? inputContainer : outputContainer).Add(port);
            }
        }

        private static readonly Type _controlPortType = typeof(ControlPort);

        private void CreateFlowPorts(UIXNodeReflection data)
        {

            var inputPortContainer = new VisualElement {name = "FlowPorts"};
            inputPortContainer.AddToClassList("FlowInPorts");
            var outputPortsContainer = new VisualElement {name = "FlowPorts"};
            outputPortsContainer.AddToClassList("FlowOutPorts");
            foreach (var portData in data.FlowPorts)
            {
                var port = PortView.Create<Edge>(Orientation.Vertical, ConvertDirection(portData.Direction), ConvertCapacity(portData.Capacity), _controlPortType);
                port.name = portData.Hash.ToString();
                port.userData = portData.Hash;
                port.portName = portData.Name;
                ((portData.Direction == PortDirection.Input) ? inputPortContainer : outputPortsContainer).Add(port);
            }
            
            if (inputPortContainer.childCount > 0) mainContainer.parent.Insert(0, inputPortContainer);
            if (outputPortsContainer.childCount > 0)
            {
                mainContainer.parent.Add(outputPortsContainer);
            }
        }

        private Direction ConvertDirection(PortDirection value) => value == PortDirection.Input ? Direction.Input : Direction.Output;
        private PortView.Capacity ConvertCapacity(PortCapacity value) => value == PortCapacity.Single ? PortView.Capacity.Single : PortView.Capacity.Multi;
        /*
            
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
        */
    }
    
    public static class NodeViewExtensions
    {
        public static INode INode(this NodeView self) => (INode) self.userData;
        public static string Id(this NodeView self) => self.INode().Id;
        public static uint Id(this PortView self) => (uint)self.userData;
    }
}