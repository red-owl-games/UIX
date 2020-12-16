using System;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueOutAttribute : Attribute
    {
        public string Name { get; set; } = null;
        
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
}