namespace RedOwl.UIX.Engine
{
    public class MultiplyNode : MathNode<float, float, float>
    {
        protected override void Calculate(Flow flow)
        {
            flow.Set(Output, flow.Get(Left) * flow.Get(Right));
        }
    }
}