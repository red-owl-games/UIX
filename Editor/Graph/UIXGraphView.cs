using RedOwl.UIX.Engine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using PortView = UnityEditor.Experimental.GraphView.Port;


namespace RedOwl.UIX.Editor
{
    public class UIXGraphView : GraphView
    {
        private IGraph _graph;
        private MiniMap _map;

        public UIXGraphView(IGraph graph)
        {
            UIXEditor.Setup(this);
            
            SetupZoom(ContentZoomer.DefaultMinScale * 0.5f, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            graphViewChanged = OnGraphViewChanged;

            CreateGridBackground();
            CreateMiniMap();
            
            Load(graph);
        }
        
        private void CreateGridBackground()
        {
            var grid = new GridBackground {name = "Grid"};
            Insert(0, grid);
        }

        private void CreateMiniMap()
        {
            _map = new MiniMap {anchored = true, maxWidth = 200, maxHeight = 100, visible = false};
            Add(_map);
        }
        
        public void Load(IGraph graph)
        {
            _graph = graph;
            
            /*
            foreach (var node in _graph.Nodes)
            {
                CreateNodeView(node);
            }

            foreach (var connection in _graph.Connections)
            {
                Debug.Log($"Creating connection {connection.Output} -> {connection.Input}");
                CreateConnection(connection);
            }
            */
        }
        
        [Callback]
        private void GeometryChangedCallback(GeometryChangedEvent evt)
        {
            _map.SetPosition(new Rect(worldBound.width - 205, 25, 200, 100));
        }
        
        [Callback]
        private void KeyDownCallback(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.M) _map.visible = !_map.visible;
        }
        
        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            /*
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    switch (element)
                    {
                        case NodeView view:
                            _graph.RemoveNode(view.INode());
                            break;
                        case Edge edge:
                            _graph.RemoveConnection(edge.output.node.INode().Id, edge.input.node.INode().Id);
                            break;
                    }

                    Debug.Log($"Removed GraphElement: {element.name} {element.title}");
                }
            }

            if (change.movedElements != null)
            {
                foreach (var element in change.movedElements)
                {
                    if (element is NodeView view) view.INode().Position = view.GetPosition().position;
                }
            }

            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    _graph.AddConnection(new Connection
                    {
                        Input = edge.input.node.INode().Id,
                        Output = edge.output.node.INode().Id,
                    });
                }
            }
            */
            return change;
        }

        /*

        public void Clean()
        {
            DeleteElements(graphElements.ToList());
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            var point = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            foreach (var possibleNode in _graph.PossibleNodes())
            {
                evt.menu.AppendAction(NodeAttribute.GetContextPath(possibleNode), a =>
                {
                    var node = (INode) Activator.CreateInstance(possibleNode);
                    node.Id = Guid.NewGuid().ToString();
                    node.Position = point;
                    _graph.AddNode(node);
                    CreateNodeView(node);
                });
            }
        }

        public override List<PortView> GetCompatiblePorts(PortView startPort, NodeAdapter nodeAdapter)
        {
            var compatible = new List<PortView>();
            ports.ForEach(p =>
            {
                if (startPort == p) return;
                if (startPort.node == p.node) return;
                if (startPort.direction == p.direction) return;
                if (p.direction == Direction.Input && !startPort.portType.IsCastableTo(p.portType, true)) return;
                if (p.direction == Direction.Output && !p.portType.IsCastableTo(startPort.portType, true)) return;
                compatible.Add(p);
            });
            return compatible;
        }

        public void CreateNodeView<T>(T node) where T : INode
        {
            var view = new UIXNodeView(node);
            AddElement(view);
        }
        
        private void CreateConnection(Connection connection)
        {
            NodeView input = null;
            NodeView output = null;
            foreach (var node in nodes.ToList())
            {
                INode model = (INode)node.userData;
                if (model == null) continue;
                if (model.Id == connection.Input) input = node;
                if (model.Id == connection.Output) output = node;
            }

            if (input == null || output == null) return;
            var inputPort = input.inputContainer.Q<PortView>();
            var outputPort = output.outputContainer.Q<PortView>();
            AddElement(outputPort.ConnectTo(inputPort));
        }
        */
    }
}