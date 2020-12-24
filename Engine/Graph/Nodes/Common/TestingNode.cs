using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Debug", Path = "Debug")]
    public class TestingNode : Node
    {
        [FlowIn(Callback = nameof(OnEnter))] public FlowPort Enter;
        [FlowOut(Callback = nameof(OnExit))] public FlowPort Exit;

        [ValueIn, ValueOut] public ValuePort String;

        public TestingNode()
        {
            Enter = new FlowPort(this);
            Exit = new FlowPort(this);

            String = new ValuePort<string>(this);
        }
        
        private IEnumerable OnEnter(IFlow flow)
        {
            Debug.Log("Enter");
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.5f);
            }
            Debug.Log("Sleep Over");
            yield return Exit;
        }
        
        private void OnExit(IFlow flow)
        {
            Debug.Log("Exit");
        }
    }
}