using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedOwl.UIX.Editor
{
    // TODO: what happens when we delete the GraphReference this is current viewing?!
    [WindowTitle("Graph Editor")]
    [USS("UIXGraphWindow", true)]
    public class UIXGraphWindow : UIXEditorWindow
    {
        #region AutoOpen

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (!(EditorUtility.InstanceIDToObject(instanceID) is GraphAsset asset)) return false;
            UIXEditor.Show<UIXGraphWindow>().Load(asset);
            return true;
        }
        
        public static void Open(GraphAsset asset) => UIXEditor.Show<UIXGraphWindow>().Load(asset);

        #endregion
        
        [SerializeReference] private GraphAsset _lastAsset;
        private UIXGraphView _view;

        private Toolbar _toolbar;

        public override void OnEnable()
        {
            base.OnEnable();
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            Undo.undoRedoPerformed += Reload;
            Reload();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Undo.undoRedoPerformed -= Reload;
        }

        private void Reload()
        {
            // TODO: this resets the view on undo/redo so we need to save the view "position"
            Load(AssetDatabase.LoadAssetAtPath<GraphAsset>(AssetDatabase.GetAssetPath(_lastAsset)));
        }

        internal void Load(GraphAsset asset)
        {
            if (asset == null) return;
            EnsureLastGraphSaved();
            _lastAsset = asset;
            EditorUtility.SetDirty(_lastAsset);
            if (_lastAsset.graph == null) _lastAsset.graph = new Graph();
            Cleanup();
            CreateView();
            CreateToolbar();
        }

        private void EnsureLastGraphSaved()
        {
            if (_lastAsset == null) return;
            EditorUtility.SetDirty(_lastAsset);
            AssetDatabase.SaveAssets();
        }

        private void Cleanup()
        {
            if (_toolbar != null) rootVisualElement.Remove(_toolbar);
            if (_view != null) rootVisualElement.Remove(_view);
        }

        private void CreateView()
        {
            _view = new UIXGraphView(_lastAsset);
            rootVisualElement.Add(_view);
            _view.StretchToParentSize();
        }

        private void CreateToolbar()
        {
            _toolbar = new Toolbar();

            // TODO: Graph Window Toolbar
            // var clearBtn = new Button(() => { }) {text = "Clear"};
            // _toolbar.Add(clearBtn);
            //
            var saveBtn = new Button(_view.SaveAsset) {text = "Save"};
            _toolbar.Add(saveBtn);
            //
            // var loadBtn = new Button(() =>
            // {
            //     _view.Clean();
            //     _view.Load(JsonUtility.FromJson<CacophonyGraph>(_save));
            // });
            // loadBtn.text = "Load";
            // toolbar.Add(loadBtn);
            
            rootVisualElement.Add(_toolbar);
        }
    }
}