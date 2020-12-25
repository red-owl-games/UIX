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
        
        private PortView CreatePortView(IPort valuePort, Orientation orientation)
        {
            var view = PortView.Create<Edge>(orientation, ConvertDirection(valuePort.Direction), ConvertCapacity(valuePort.Capacity), valuePort.ValueType);
            view.name = valuePort.Id.Port;
            view.userData = valuePort;
            view.portName = valuePort.Name;
            return view;
        }
        
        private void CreateValuePorts(INode node)
        {
            foreach (var valuePort in node.ValueInPorts.Values)
            {
                inputContainer.Add(CreatePortView(valuePort, Orientation.Horizontal));
            }
            
            foreach (var valuePort in node.ValueOutPorts.Values)
            {
                outputContainer.Add(CreatePortView(valuePort, Orientation.Horizontal));
            }
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
            foreach (var flowPort in node.FlowInPorts.Values)
            {
                FlowInPortContainer.Add(CreatePortView(flowPort, Orientation.Vertical));
            }
            
            foreach (var flowPort in node.FlowOutPorts.Values)
            {
                FlowOutPortContainer.Add(CreatePortView(flowPort, Orientation.Vertical));
            }
        }

        private void AttachFlowPortContainers()
        {
            if (FlowInPortContainer.childCount > 0) mainContainer.parent.Insert(0, FlowInPortContainer);
            if (FlowOutPortContainer.childCount > 0) mainContainer.parent.Add(FlowOutPortContainer);
        }

        private Direction ConvertDirection(PortDirection value) => value == PortDirection.Input ? Direction.Input : Direction.Output;
        private PortView.Capacity ConvertCapacity(PortCapacity value) => value == PortCapacity.Single ? PortView.Capacity.Single : PortView.Capacity.Multi;

        public IEnumerable<Edge> CreateConnections(Dictionary<string, UIXNodeView> cache)
        {
            foreach (var valuePort in Model.ValueInPorts.Values)
            {
                foreach (var connection in valuePort.Connections)
                {
                    if (!cache.TryGetValue(connection.Node, out var output)) continue;
                    var inputPort = inputContainer.Q<PortView>(valuePort.Id.Port);
                    var outputPort = output.outputContainer.Q<PortView>(connection.Port);
                    if (inputPort != null && outputPort != null) 
                        yield return outputPort.ConnectTo(inputPort);
                }
            }

            foreach (var flowPort in Model.FlowOutPorts.Values)
            {
                foreach (var connection in flowPort.Connections)
                {
                    if (!cache.TryGetValue(connection.Node, out var inputNode)) continue;
                    var inputPort = inputNode.FlowInPortContainer.Q<PortView>(connection.Port);
                    var outputPort = FlowOutPortContainer.Q<PortView>(flowPort.Id.Port);
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