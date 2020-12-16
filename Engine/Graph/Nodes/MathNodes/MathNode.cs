// using System.Collections;
//
// namespace RedOwl.UIX.Engine
// {
//     [Node("Math", Path = "Math")]
//     public abstract class MathNode<T1, T2, T3> : Node
//     {
//         [ValueIn] public T1 Left { get; } = default;
//         [ValueIn] public T2 Right { get; } = default;
//
//         [ValueOut] public T3 Output { get; } = default;
//
//         [FlowIn]
//         protected void Enter(Flow flow)
//         {
//             Calculate(flow);
//             //yield return Exit;
//         }
//
//         [FlowOut] public ControlPort Exit;
//
//         protected abstract void Calculate(Flow flow);
//     }
// }