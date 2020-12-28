namespace RedOwl.UIX.Engine
{
    public class OperationNode : MathNode<double, double, double>
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        public Operation operation = Operation.Add;
        
        protected override void Calculate(IFlow flow)
        {
            switch (operation)
            {
                case Operation.Add:
                    flow.Set(Output, flow.Get<double>(Left) + flow.Get<double>(Right));
                    break;
                case Operation.Subtract:
                    flow.Set(Output, flow.Get<double>(Left) - flow.Get<double>(Right));
                    break;
                case Operation.Multiply:
                    flow.Set(Output, flow.Get<double>(Left) * flow.Get<double>(Right));
                    break;
                case Operation.Divide:
                    // TODO: protect against / by 0
                    flow.Set(Output, flow.Get<double>(Left) / flow.Get<double>(Right));
                    break;
            }
            
        }
    }
}