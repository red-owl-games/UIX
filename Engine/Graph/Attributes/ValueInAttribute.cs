using System;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An input port exposed on a Node
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueInAttribute : Attribute
    {
        public string Name { get; set; }

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;

        public ValueInAttribute(string name = null)
        {
            Name = name;
        }
    }
}