using System;
using System.Collections.Generic;
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
        public VisualElement FlowInPortContainer { get; private set; }
        public VisualElement FlowOutPortContainer { get; private set; }
        public INode Model => (INode) userData;

        public UIXNodeView(INode node)
        {
            userData = node;
            Initialize(node);
        }

        private void Initialize(INode node)
        {
            name = node.NodeId;
            title = node.NodeTitle;
            SetPosition(node.NodeRect);
            // TODO: map capabilities AND if we use Resizeable we'll need to update the data's Rect with the changes.
            //capabilities -= Capabilities.Collapsible;
            //capabilities = Capabilities.Movable;
            //capabilities = Capabilities.Resizable;
            //this.IsResizable();
            //style.width = Model.NodeRect.width;
            //style.height = Model.NodeRect.height;

            CreateBody(node);
            CreateFlowPortContainers();
            CreateFlowPorts(node);
            AttachFlowPortContainers();
            CreateValuePorts(node);
            RefreshExpandedState();
            RefreshPorts();
        }
        
        private void CreateBody(INode node)
        {
            UIXEditor.DrawInto(node, extensionContainer);
        }
        
        private void CreateValuePorts(INode node)
        {
            foreach (var valuePort in node.ValuePorts.Values)
            {
                switch (valuePort.Direction)
                {
                    case PortDirection.Input:
                        CreateValuePort(valuePort, Direction.Input);
                        break;
                    case PortDirection.Output:
                        CreateValuePort(valuePort, Direction.Output);
                        break;
                    case PortDirection.InOut:
                        CreateValuePort(valuePort, Direction.Input);
                        CreateValuePort(valuePort, Direction.Output);
                        break;
                }
            }
        }

        private void CreateValuePort(ValuePort valuePort, Direction direction)
        {
            var port = PortView.Create<Edge>(Orientation.Horizontal, direction, ConvertCapacity(valuePort.Capacity), valuePort.PortType);
            port.name = valuePort.PortId;
            port.userData = valuePort;
            port.portName = valuePort.Name;
            (direction == Direction.Input ? inputContainer : outputContainer).Add(port);
        }

        private void CreateFlowPortContainers()
        {
            FlowInPortContainer = new VisualElement {name = "FlowPorts"};
            FlowInPortContainer.AddToClassList("FlowInPorts");
            FlowOutPortContainer = new VisualElement {name = "FlowPorts"};
            FlowOutPortContainer.AddToClassList("FlowOutPorts");
        }

        private void CreateFlowPorts(INode node)
        {
            foreach (var flowPort in node.FlowPorts.Values)
            {
                switch (flowPort.Direction)
                {
                    case PortDirection.Input:
                        CreateFlowPort(flowPort);
                        break;
                    case PortDirection.Output:
                        CreateFlowPort(flowPort);
                        break;
                    case PortDirection.InOut:
                        // TODO: Implement
                        Debug.LogWarning("Flow Port InOut is Not Implemented");
                        break;
                }
            }
        }

        private void AttachFlowPortContainers()
        {
            if (FlowInPortContainer.childCount > 0) mainContainer.parent.Insert(0, FlowInPortContainer);
            if (FlowOutPortContainer.childCount > 0) mainContainer.parent.Add(FlowOutPortContainer);
        }
        
        private static readonly Type FlowPortType = typeof(FlowPort);
        private void CreateFlowPort(FlowPort flowPort)
        {
            var port = PortView.Create<Edge>(Orientation.Vertical, ConvertDirection(flowPort.Direction), ConvertCapacity(flowPort.Capacity), FlowPortType);
            port.name = flowPort.PortId;
            port.userData = flowPort;
            port.portName = flowPort.Name;
            ((flowPort.Direction == PortDirection.Input) ? FlowInPortContainer : FlowOutPortContainer).Add(port);
        }

        private Direction ConvertDirection(PortDirection value) => value == PortDirection.Input ? Direction.Input : Direction.Output;
        private PortView.Capacity ConvertCapacity(PortCapacity value) => value == PortCapacity.Single ? PortView.Capacity.Single : PortView.Capacity.Multi;

        public IEnumerable<Edge> CreateConnections(Dictionary<string, UIXNodeView> cache)
        {
            foreach (var valuePort in Model.ValuePorts.Values)
            {
                foreach (var connection in valuePort.Connections)
                {
                    if (!cache.TryGetValue(connection.Node, out var output)) continue;
                    var inputPort = inputContainer.Q<PortView>(valuePort.PortId);
                    var outputPort = output.outputContainer.Q<PortView>(connection.Port);
                    if (inputPort != null && outputPort != null) 
                        yield return outputPort.ConnectTo(inputPort);
                }
            }

            foreach (var flowPort in Model.FlowPorts.Values)
            {
                foreach (var connection in flowPort.Connections)
                {
                    if (!cache.TryGetValue(connection.Node, out var output)) continue;
                    var inputPort = FlowOutPortContainer.Q<PortView>(flowPort.PortId);
                    var outputPort = output.FlowInPortContainer.Q<PortView>(connection.Port);
                    if (inputPort != null && outputPort != null) 
                        yield return outputPort.ConnectTo(inputPort);
                }
            }
        }
    }
    
    public static class NodeViewExtensions
    {
        //public static INode INode(this NodeView self) => (INode) self.userData;
        // public static string Id(this NodeView self) => self.INode().NodeId;
        // public static uint Id(this PortView self) => (uint)self.userData;
    }
}