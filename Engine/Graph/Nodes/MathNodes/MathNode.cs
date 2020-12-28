using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Node("Math", Path = "Math")]
    public abstract class MathNode<TInput, TOutput> : Node
    {
        [FlowIn(Callback = nameof(OnEnter))] public FlowPort Enter;
        [FlowOut] public FlowPort Exit;
        
        [ValueIn] public ValuePort<TInput> Input;

        [ValueOut] public ValuePort<TOutput> Output;

        protected MathNode()
        {
            Enter = new FlowPort(this);
            Exit = new FlowPort(this);

            Input = new ValuePort<TInput>(this);
            Output = new ValuePort<TOutput>(this);
        }

        protected void OnEnter(Flow flow)
        {
            Calculate(flow);
        }
        
        protected abstract void Calculate(Flow flow);
    }
    
    [Node("Math", Path = "Math")]
    public abstract class MathNode<TLeft, TRight, TOutput> : Node
    {
        [FlowIn(Callback = nameof(OnEnter))] public FlowPort Enter;
        [FlowOut] public FlowPort Exit;
        
        [ValueIn] public ValuePort<TLeft> Left;
        [ValueIn] public ValuePort<TRight> Right;

        [ValueOut] public ValuePort<TOutput> Output;

        protected MathNode()
        {
            Enter = new FlowPort(this);
            Exit = new FlowPort(this);

            Left = new ValuePort<TLeft>(this);
            Right = new ValuePort<TRight>(this);

            Output = new ValuePort<TOutput>(this);
        }

        protected void OnEnter(Flow flow)
        {
            Calculate(flow);
        }
        
        protected abstract void Calculate(Flow flow);
    }
}