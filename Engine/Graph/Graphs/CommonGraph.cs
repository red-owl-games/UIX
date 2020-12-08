using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Graph", fileName = "Graph")]
    [Graph("Math", "Flow", "Common")]
    public class CommonGraph : Graph
    {
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonSizes.Large)]
        #endif
        public void Execute()
        {
            foreach (var start in GetNodes<StartNode>())
            {
                Debug.Log($"Executing @ {start.Id}");
                new Flow(this).Execute(start);
            }
        }
    }
}