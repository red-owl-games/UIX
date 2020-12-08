using System;
using System.Collections.Generic;
using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using PortView = UnityEditor.Experimental.GraphView.Port;


namespace RedOwl.UIX.Editor
{
    public class UIXGraphView : GraphView
    {
        private SerializedObject _serializedGraph;
        private readonly IGraph _graph;
        public IGraph Graph => _graph;
        
        private MiniMap _map;
        private UIXGraphSearchProvider _search;

        public UIXGraphView(Graph graph)
        {
            if (graph == null)
            {
                CreateGridBackground();
                return;
            }
            
            // This is to prepare for Undo's
            _serializedGraph = new SerializedObject(graph);
            _graph = (IGraph)graph;

            UIXEditor.Setup(this);
            
            SetupZoom(ContentZoomer.DefaultMinScale * 0.5f, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            graphViewChanged = OnGraphViewChanged;

            CreateGridBackground();
            CreateMiniMap();
            CreateSearch();

            LoadNodes();
            LoadConnections();
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

        private void CreateSearch()
        {
            _search = ScriptableObject.CreateInstance<UIXGraphSearchProvider>();
            _search.Initialize(this);
            nodeCreationRequest = ctx => SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), _search);
        }

        public void LoadNodes()
        {
            foreach (var node in _graph.Nodes)
            {
                CreateNodeView(node);
            }
        }

        public void LoadConnections()
        {
            foreach (var connection in _graph.Connections)
            {
                //Debug.Log($"Creating connection {connection.Output} -> {connection.Input}");
                CreateConnection(connection);
            }
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
                            _graph.RemoveConnection(GetConnection(edge));
                            break;
                    }

                    //Debug.Log($"Removed GraphElement: {element.name} {element.title}");
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
                    _graph.AddConnection(GetConnection(edge));
                }
            }

            return change;
        }

        private Connection GetConnection(Edge edge)
        {
            return new Connection
            {
                Input = new Slot
                {
                    Node = edge.input.node.Id(),
                    Port = edge.input.Id()
                },
                Output = new Slot
                {
                    Node = edge.output.node.Id(),
                    Port = edge.output.Id()
                }
            };
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
        
        internal void CreateNode(UIXNodeReflection data, Vector2 position)
        {
            var node = (INode)Activator.CreateInstance(data.Type);
            var window = EditorWindow.GetWindow<UIXGraphWindow>();
            node.Position = window.rootVisualElement.ChangeCoordinatesTo(contentViewContainer, position - window.position.position - new Vector2(3, 26));
            _graph.AddNode(node);
            CreateNodeView(node);
        }

        private void CreateNodeView<T>(T node) where T : INode
        {
            if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) return;
            AddElement(new UIXNodeView(node, data));
        }
        
        private void CreateConnection(Connection connection)
        {
            NodeView input = this.Q<NodeView>(connection.Input.Node);
            NodeView output = this.Q<NodeView>(connection.Output.Node);
            if (input == null || output == null) return;
            var inputPort = input.Q<PortView>(connection.Input.Port.ToString());
            var outputPort = output.Q<PortView>(connection.Output.Port.ToString());
            if (inputPort == null || outputPort == null) return;
            AddElement(outputPort.ConnectTo(inputPort));
        }

        /*

        public void Clean()
        {
            DeleteElements(graphElements.ToList());
        }


        

        */
    }
}