using System;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An input port exposed on a Node
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueInAttribute : Attribute
    {
        public string Name { get; set; } = null;

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
    }
}