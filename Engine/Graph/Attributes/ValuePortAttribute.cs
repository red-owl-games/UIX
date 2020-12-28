using System;

namespace RedOwl.UIX.Engine
{
    public interface IValuePortAttribute
    {
        string Name { get; }
        PortDirection Direction { get; }
        PortCapacity Capacity { get; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueInAttribute : Attribute, IValuePortAttribute
    {
        public string Name { get; set; } = null;
        public PortDirection Direction => PortDirection.Input;
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueOutAttribute : Attribute, IValuePortAttribute
    {
        public string Name { get; set; } = null;
        public PortDirection Direction => PortDirection.Output;
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
}