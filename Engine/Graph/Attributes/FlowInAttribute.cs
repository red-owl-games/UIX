using System;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlowInAttribute : Attribute
    {
        public string Name { get; set; } = null;

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
}