using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Graph", fileName = "Graph")]
    public class GraphAsset : ScriptableObject
    {
        [SerializeReference]
#if !ODIN_INSPECTOR
        [ImplementationPicker(typeof(IGraph))]
#else
        [Sirenix.OdinInspector.InlineEditor, Sirenix.OdinInspector.HideLabel]
#endif
        public IGraph Graph;
    }
}