using System;

namespace RedOwl.UIX.Engine
{
    public class FlowInAttribute : Attribute
    {
        public string Name { get; set; }

        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;
        
        public FlowInAttribute(string name = null)
        {
            Name = name;
        }
    }
}