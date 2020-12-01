using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedOwl.UIX.Editor
{
    [WindowTitle("Graph Editor")]
    [USS("UIXGraphWindow", true)]
    public class UIXGraphWindow : UIXEditorWindow
    {
        private UIXGraphView _view;
        
        private void Load(IGraph graph)
        {
            _view = new UIXGraphView(graph);
            _view.StretchToParentSize();
            rootVisualElement.Add(_view);
        }
        
        #region AutoOpen

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (!(EditorUtility.InstanceIDToObject(instanceID) is GraphReference reference)) return false;
            UIXEditor.Show<UIXGraphWindow>().Load(reference.Graph);
            return true;
        }

        #endregion
        /*
        
        
        public override void OnEnable()
        {
            base.OnEnable();
            CreateView();
            CreateToolbar();
        }

        private void CreateToolbar()
        {
            var toolbar = new Toolbar();
            
            // var clearBtn = new Button(() =>
            // {
            //     _view.Clean();
            //     _view.Load(new CacophonyGraph());
            // });
            // clearBtn.text = "Clear";
            // toolbar.Add(clearBtn);
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
            
            rootVisualElement.Add(toolbar);
        }

        private void CreateView()
        {
            _view = new UIXGraphView(new T());
            _view.StretchToParentSize();
            rootVisualElement.Add(_view);
        }

        public override void OnDisable()
        {
            rootVisualElement.Remove(_view);
        }
        */
    }
}