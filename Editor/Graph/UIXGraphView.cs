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
            _graph.Definition();
            _graph.Initialize();
            _nodeViewCache = new Dictionary<string, UIXNodeView>(_graph.NodeCount);

            UIXEditor.Setup(this);
            
            SetupZoom(ContentZoomer.DefaultMinScale * 0.5f, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            graphViewChanged = OnGraphViewChanged;

            CreateGridBackground();
            CreateMiniMap();
            CreateSearch();

            // TODO: If graph.NodeCount > 100 we need a loading bar and maybe an async process that does the below
            CreateNodeViews();
            CreateConnections();
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

        private void CreateNodeViews()
        {
            foreach (var node in _graph.Nodes)
            {
                CreateNodeView(node);
            }
        }

        private void CreateConnections()
        {
            // TODO: Needs Refactor
            foreach (var node in _graph.Nodes)
            {
                var view = _nodeViewCache[node.NodeId];
                foreach (var valueIn in node.ValueInPorts.Values)
                {
                    foreach (var connection in _graph.ValueInConnections.SafeGet(valueIn.Id))
                    {
                        if (!_nodeViewCache.TryGetValue(connection.Node, out var outputView)) continue;
                        var inputPort = view.inputContainer.Q<PortView>(valueIn.Id.Port);
                        var outputPort = outputView.outputContainer.Q<PortView>(connection.Port);
                        if (inputPort != null && outputPort != null) 
                            AddElement(outputPort.ConnectTo(inputPort));
                    }
                }
                if (!(node is IFlowNode flowNode)) continue;
                foreach (var flowOut in flowNode.FlowOutPorts.Values)
                {
                    foreach (var connection in _graph.FlowOutConnections.SafeGet(flowOut.Id))
                    {
                        if (!_nodeViewCache.TryGetValue(connection.Node, out var inputView)) continue;
                        var inputPort = inputView.FlowInPortContainer.Q<PortView>(connection.Port);
                        var outputPort = view.FlowOutPortContainer.Q<PortView>(flowOut.Id.Port);
                        if (inputPort != null && outputPort != null)
                            AddElement(outputPort.ConnectTo(inputPort));
                    }
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
            bool graphRedraw = false;
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    switch (element)
                    {
                        case UIXNodeView view:
                            changeMade = true;
                            graphRedraw = true; // Hack that cleans up the flow port connections
                            _nodeViewCache.Remove(view.Model.NodeId);
                            _graph.Remove(view.Model);
                            break;
                        case Edge edge:
                            changeMade = true;
                            _graph.Disconnect((IPort)edge.output.userData, (IPort)edge.input.userData);
                            break;
                        default:
                            Debug.LogWarning($"Unhandeled GraphElement Removed: {element.GetType().FullName} | {element.name} | {element.title}");
                            break;
                    }
                }
            }
            
            if (change.movedElements != null)
            {
                foreach (var element in change.movedElements)
                {
                    switch (element)
                    {
                        case UIXNodeView view:
                            changeMade = true; 
                            view.Model.NodeRect = view.GetPosition();
                            break;
                        default:
                            Debug.LogWarning($"Unhandeled GraphElement Moved: {element.GetType().FullName} | {element.name} | {element.title}");
                            break;
                    }
                }
            }
            
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    changeMade = true;
                    _graph.Connect((IPort)edge.output.userData, (IPort)edge.input.userData);
                }
            }

            if (changeMade)
            {
                SaveAsset();
            }

            if (graphRedraw)
            {
                // TODO: this is a hack that cleans up the flow port connections - we may want to try and query for them and just delete them instead
                UIXEditor.Show<UIXGraphWindow>().Load(_graphAsset);
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
            _graph.Add(node); // Definition & Initialize are called here
            var window = EditorWindow.GetWindow<UIXGraphWindow>();
            node.NodePosition = window.rootVisualElement.ChangeCoordinatesTo(contentViewContainer, position - window.position.position - new Vector2(3, 26));
            CreateNodeView(node);
            SaveAsset();
        }

        private Dictionary<string, UIXNodeView> _nodeViewCache;
        private void CreateNodeView<T>(T node) where T : INode
        {
            var element = new UIXNodeView(node);
            _nodeViewCache.Add(node.NodeId, element);
            AddElement(element);
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