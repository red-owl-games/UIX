using System;

namespace RedOwl.UIX.Engine
{
    public interface IFlowPortAttribute
    {
        string Name { get; }
        PortDirection Direction { get; }
        PortCapacity Capacity { get; }
        string Callback { get; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlowInAttribute : Attribute, IFlowPortAttribute
    {
        public string Name { get; set; } = null;
        public PortDirection Direction => PortDirection.Input;
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
        
        public string Callback { get; set; } = string.Empty;
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlowOutAttribute : Attribute, IFlowPortAttribute
    {
        public string Name { get; set; } = null;
        public PortDirection Direction => PortDirection.Output;
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
        
        public string Callback { get; set; } = string.Empty;
    }
}