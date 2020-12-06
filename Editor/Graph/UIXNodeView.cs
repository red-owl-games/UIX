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
            title = data.Name;
            SetPosition(new Rect(Model.Position, data.Size));

            CreateBody();
            CreatePorts(data);
            RefreshExpandedState();
            RefreshPorts();
        }
        
        private void CreateBody()
        {
            UIXEditor.DrawInto(Model, extensionContainer);
        }
        
        private void CreatePorts(UIXNodeReflection data)
        {
            // TODO: a Custom UIXPortView might be needed to support things
            foreach (var portData in data.Ports)
            {
                var port = PortView.Create<Edge>(Orientation.Horizontal, portData.Direction, portData.Capacity, portData.Type);
                port.portName = portData.Name;
                (portData.Direction == Direction.Input ? inputContainer : outputContainer).Add(port);
            }
        }
        
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
    }
}