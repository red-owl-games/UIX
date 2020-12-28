namespace RedOwl.UIX.Engine
{
    public class OrNode : MathNode<bool, bool, bool>
    {
        protected override void Calculate(IFlow flow)
        {
            flow.Set(Output, flow.Get<bool>(Left) || flow.Get<bool>(Right));
        }
    }
}