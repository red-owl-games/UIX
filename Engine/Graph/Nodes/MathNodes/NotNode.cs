namespace RedOwl.UIX.Engine
{
    public class NotNode : MathNode<bool, bool>
    {
        protected override void Calculate(IFlow flow)
        {
            Output.Value = !Input.Value;
        }
    }
}