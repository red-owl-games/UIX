using RedOwl.UIX.Engine;
using UnityEditor;
using UnityEngine;

namespace RedOwl.UIX.Editor
{
#if !ODIN_INSPECTOR
    [CustomEditor(typeof(GraphAsset))] 
    public class UIXGraphAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            if (GUILayout.Button("Open Editor", GUILayout.Height(40))) UIXGraphWindow.Open((GraphAsset) serializedObject.targetObject);
        }
    }
#endif
}