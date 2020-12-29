using System.Collections;

namespace RedOwl.UIX.Engine
{
    public abstract class StateNode : Node
    {
        [FlowIn(Callback=nameof(HandleEnter))] public FlowPort Enter;
        [FlowOut(Callback=nameof(OnExit))] public FlowPort Exit;

        protected StateNode()
        {
            Enter = new FlowPort(this);
            Exit = new FlowPort(this);
        }

        protected IEnumerator HandleEnter(IFlow flow)
        {
            OnEnter(flow);
            var enumerator = OnUpdate(flow);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            yield return Exit;
        }

        protected abstract void OnEnter(IFlow flow);
        protected abstract IEnumerator OnUpdate(IFlow flow);
        protected abstract void OnExit(IFlow flow);
    }
}