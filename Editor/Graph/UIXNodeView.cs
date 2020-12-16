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
            title = Model.Title;
            SetPosition(Model.GraphRect);

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
                //Debug.Log($"Creating Value Port '{portData.Name}' of type '{portData.PortType(Model).FullName}'");
                var port = PortView.Create<Edge>(Orientation.Horizontal, ConvertDirection(portData.Direction), ConvertCapacity(portData.Capacity), portData.PortType(Model));
                port.name = portData.PortId(Model);
                port.userData = portData.Port(Model);
                port.portName = portData.Name;
                (portData.Direction == PortDirection.Input ? inputContainer : outputContainer).Add(port);
            }
        }

        private static readonly Type FlowPortType = typeof(FlowPort);

        private void CreateFlowPorts(UIXNodeReflection data)
        {
            var inputPortContainer = new VisualElement {name = "FlowPorts"};
            inputPortContainer.AddToClassList("FlowInPorts");
            var outputPortsContainer = new VisualElement {name = "FlowPorts"};
            outputPortsContainer.AddToClassList("FlowOutPorts");
            foreach (var portData in data.FlowPorts)
            {
                var port = PortView.Create<Edge>(Orientation.Vertical, ConvertDirection(portData.Direction), ConvertCapacity(portData.Capacity), FlowPortType);
                port.name = portData.PortId(Model);
                port.userData = portData.Port(Model);
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
    }
    
    public static class NodeViewExtensions
    {
        public static INode INode(this NodeView self) => (INode) self.userData;
        public static string Id(this NodeView self) => self.INode().Id;
        public static uint Id(this PortView self) => (uint)self.userData;
    }
}