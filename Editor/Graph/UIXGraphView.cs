using System;
using System.Collections.Generic;
using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using Port = RedOwl.UIX.Engine.Port;
using PortView = UnityEditor.Experimental.GraphView.Port;


namespace RedOwl.UIX.Editor
{
    public class UIXGraphView : GraphView
    {
        private GraphAsset _graphAsset;
        private SerializedObject _serializedGraph;
        private readonly IGraph _graph;
        public IGraph Graph => _graph;
        
        private MiniMap _map;
        private UIXGraphSearchProvider _search;

        public UIXGraphView(GraphAsset asset)
        {
            if (asset == null)
            {
                CreateGridBackground();
                return;
            }

            _graphAsset = asset;
            // This is to prepare for Undo's
            _serializedGraph = new SerializedObject(asset);
            _graph = asset.graph;

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
            LoadValueConnections();
            LoadFlowConnections();
        }

        private void LoadValueConnections()
        {
            foreach (var values in _graph.ValueConnections)
            {
                var input = values.Key;
                foreach (var output in values.Value)
                {
                    //Debug.Log($"Creating connection {connection.Output} -> {connection.Input}");
                    CreateConnection(output, input);
                }
            }
        }

        private void LoadFlowConnections()
        {
            foreach (var flows in _graph.FlowConnections)
            {
                var output = flows.Key;
                foreach (var input in flows.Value)
                {
                    //Debug.Log($"Creating connection {connection.Output} -> {connection.Input}");
                    CreateConnection(output, input);
                }
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
            Undo.RecordObject(_graphAsset, "Graph Edit");
            bool changeMade = false;
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    switch (element)
                    {
                        case NodeView view:
                            changeMade = true;
                            _graph.Remove(view.INode());
                            break;
                        case Edge edge:
                            changeMade = true;
                            _graph.Disconnect((Port)edge.output.userData, (Port)edge.input.userData);
                            break;
                    }

                    //Debug.Log($"Removed GraphElement: {element.name} {element.title}");
                }
            }
            
            if (change.movedElements != null)
            {
                foreach (var element in change.movedElements)
                {
                    if (element is NodeView view)
                    {
                        changeMade = true;
                        view.INode().GraphRect = view.GetPosition();
                    }
                }
            }
            
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    changeMade = true;
                    _graph.Connect((Port)edge.output.userData, (Port)edge.input.userData);
                }
            }

            if (changeMade)
            {
                SaveAsset();
            }
            return change;
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
            Undo.RecordObject(_graphAsset, "Create Node");
            var node = (INode)Activator.CreateInstance(data.Type);
            var window = EditorWindow.GetWindow<UIXGraphWindow>();
            node.GraphPosition = window.rootVisualElement.ChangeCoordinatesTo(contentViewContainer, position - window.position.position - new Vector2(3, 26));
            _graph.Add(node);
            CreateNodeView(node);
            SaveAsset();
        }

        private void CreateNodeView<T>(T node) where T : INode
        {
            if (!UIXGraphReflector.NodeCache.Get(node.GetType(), out var data)) return;
            AddElement(new UIXNodeView(node, data));
        }
        
        private void CreateConnection(PortId output, PortId input)
        {
            NodeView inputNode = this.Q<NodeView>(input.Node);
            NodeView outputNode = this.Q<NodeView>(output.Node);
            if (inputNode == null || outputNode == null) return;
            var inputPort = inputNode.Q<PortView>(input.Port);
            var outputPort = outputNode.Q<PortView>(output.Port);
            if (inputPort == null || outputPort == null) return;
            AddElement(outputPort.ConnectTo(inputPort));
        }

        internal void SaveAsset()
        {
            // TODO: Purge Keys from connections tables that have no values in their port collection?
            EditorUtility.SetDirty(_graphAsset);
            AssetDatabase.SaveAssets();
        }

        /*

        public void Clean()
        {
            DeleteElements(graphElements.ToList());
        }

        */
    }
}