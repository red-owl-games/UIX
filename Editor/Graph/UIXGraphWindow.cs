using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedOwl.UIX.Editor
{
    
    [WindowTitle("Graph Editor")]
    [USS("UIXGraphWindow", true)]
    public class UIXGraphWindow : UIXEditorWindow
    {
        #region AutoOpen

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (!(EditorUtility.InstanceIDToObject(instanceID) is Graph reference)) return false;
            UIXEditor.Show<UIXGraphWindow>().Load(reference);
            return true;
        }

        #endregion
        
        [SerializeReference] private Graph _lastGraph;
        private UIXGraphView _view;

        private Toolbar _toolbar;

        public override void OnEnable()
        {
            base.OnEnable();
            Load(_lastGraph);
        }

        private void Load(Graph asset)
        {
            if (asset == null) return;
            _lastGraph = asset;
            EditorUtility.SetDirty(_lastGraph);
            Cleanup();
            CreateView();
            CreateToolbar();
        }

        private void Cleanup()
        {
            if (_toolbar != null) rootVisualElement.Remove(_toolbar);
            if (_view != null) rootVisualElement.Remove(_view);
        }

        private void CreateView()
        {
            _view = new UIXGraphView(_lastGraph);
            rootVisualElement.Add(_view);
            _view.StretchToParentSize();
        }

        private void CreateToolbar()
        {
            _toolbar = new Toolbar();

            // var clearBtn = new Button(() => { }) {text = "Clear"};
            // _toolbar.Add(clearBtn);
            //
            // var saveBtn = new Button(() =>
            // {
            //     _save = JsonUtility.ToJson(_view.Save());
            //     Debug.Log(_save);
            // });
            // saveBtn.text = "Save";
            // toolbar.Add(saveBtn);
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