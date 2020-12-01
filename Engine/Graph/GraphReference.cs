using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Graph", fileName = "Graph")]
    public class GraphReference : ScriptableObject
    {
        [SerializeReference]
        public IGraph Graph;
    }
}