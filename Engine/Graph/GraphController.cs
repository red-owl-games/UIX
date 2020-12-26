using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideMonoScript]
#endif
    public class GraphController : MonoBehaviour
    {
        [Flags]
        public enum Stages
        {
            None = 0b_00000,
            Awake = 0b_00001,
            Start = 0b_00010,
            Update = 0b_00100,
            FixedUpdate = 0b_01000,
            LateUpdate = 0b_10000,
            All = 0b_11111
        }
        
        public GraphAsset graphAsset;
        public Stages stage;
        
        private IGraph Graph => graphAsset.graph;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonSizes.Large), Sirenix.OdinInspector.DisableInPlayMode]
#endif
        private void Execute()
        {
            if (graphAsset == null || graphAsset.graph == null) return;
            Graph.Initialize();
            Debug.Log($"Execute Graph '{graphAsset.name}'");
            new Flow(Graph).Execute();
            //StartCoroutine(WalkAsync(flow));
            // EditorCoroutineUtility.StartCoroutineOwnerless(WalkAsync(flow));
            //while(WalkAsync(flow).MoveNext()){}
        }

        private IFlow _awakeFlow;
        private IFlow _startFlow;
        private IFlow _updateFlow;
        private IFlow _fixedUpdateFlow;
        private IFlow _lateUpdateFlow;
        
        private void Awake()
        {
            Graph.Initialize(); // TODO: should this be implicit somewhere?
            _awakeFlow = new Flow<StartNode>(Graph);
            _startFlow = new Flow<StartNode>(Graph);
            _updateFlow = new Flow<UpdateNode>(Graph);
            _fixedUpdateFlow = new Flow<FixedUpdateNode>(Graph);
            _lateUpdateFlow = new Flow<LateUpdateNode>(Graph);
            
            if (stage.HasFlag(Stages.Awake)) _awakeFlow.Execute();
        }

        private void Start()
        {
            if (stage.HasFlag(Stages.Start)) 
            {
                _startFlow.Execute();
            }
        }

        private void Update()
        {
            if (stage.HasFlag(Stages.Update)) _updateFlow.Execute();
        }

        private void FixedUpdate()
        {
            if (stage.HasFlag(Stages.FixedUpdate)) _fixedUpdateFlow.Execute();
        }

        private void LateUpdate()
        {
            if (stage.HasFlag(Stages.LateUpdate)) _lateUpdateFlow.Execute();
        }
    }
}