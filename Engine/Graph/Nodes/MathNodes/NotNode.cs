namespace RedOwl.UIX.Engine
{
    public class NotNode : MathNode<bool, bool>
    {
        protected override void Calculate(Flow flow)
        {
            flow.Set(Output, !flow.Get<bool>(Input));
        }
    }
}