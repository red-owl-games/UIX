using Unity.Mathematics;

namespace RedOwl.UIX.Engine
{
    public class SignNode : MathNode<double, double>
    {
        protected override void Calculate(IFlow flow)
        {
            flow.Set(Output, math.sign(flow.Get<double>(Input)));
        }
    }
}
