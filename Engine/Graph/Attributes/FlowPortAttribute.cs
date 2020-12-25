using System;

namespace RedOwl.UIX.Engine
{
    public abstract class FlowPortAttribute : Attribute {}
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlowInAttribute : FlowPortAttribute
    {
        public string Name { get; set; } = null;
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
        
        public string Callback { get; set; } = null;
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlowOutAttribute : FlowPortAttribute
    {
        public string Name { get; set; } = null;

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
        
        public string Callback { get; set; } = null;
    }
}