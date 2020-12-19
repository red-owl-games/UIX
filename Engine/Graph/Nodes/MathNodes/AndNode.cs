namespace RedOwl.UIX.Engine
{
    public class AndNode : MathNode<bool, bool, bool>
    {
        protected override void Calculate(Flow flow)
        {
            flow.Set(Output, flow.Get<bool>(Left) && flow.Get<bool>(Right));
        }
    }
}