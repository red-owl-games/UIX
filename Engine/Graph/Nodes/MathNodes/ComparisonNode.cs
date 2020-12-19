using Unity.Mathematics;

namespace RedOwl.UIX.Engine
{
    public class ComparisonNode : MathNode<double, double, bool>
    {
        public enum Comparison
        {
            Equal,
            NotEqual,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThenOrEqual
        }

        public Comparison comparison = Comparison.Equal;
        
        protected override void Calculate(Flow flow)
        {
            switch (comparison)
            {
                case Comparison.Equal:
                    flow.Set(Output, Approximately(flow.Get<double>(Left), flow.Get<double>(Right)));
                    break;
                case Comparison.NotEqual:
                    flow.Set(Output, !Approximately(flow.Get<double>(Left), flow.Get<double>(Right)));
                    break;
                case Comparison.GreaterThan:
                    flow.Set(Output, flow.Get<double>(Left) > flow.Get<double>(Right));
                    break;
                case Comparison.GreaterThanOrEqual:
                    flow.Set(Output, flow.Get<double>(Left) >= flow.Get<double>(Right));
                    break;
                case Comparison.LessThan:
                    flow.Set(Output, flow.Get<double>(Left) < flow.Get<double>(Right));
                    break;
                case Comparison.LessThenOrEqual:
                    flow.Set(Output, flow.Get<double>(Left) <= flow.Get<double>(Right));
                    break;
            }
        }

        private bool Approximately(double a, double b) => math.abs(b - a) < math.EPSILON_DBL;
    }
}