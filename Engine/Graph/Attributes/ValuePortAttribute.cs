using System;

namespace RedOwl.UIX.Engine
{
    public abstract class ValuePortAttribute : Attribute {}
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueInAttribute : ValuePortAttribute
    {
        public string Name { get; set; } = null;

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueOutAttribute : ValuePortAttribute
    {
        public string Name { get; set; } = null;
        
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
}