using System;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueInOutAttribute : Attribute
    {
        public string InName { get; set; } = null;
        public string OutName { get; set; } = null;

        public PortCapacity InCapacity { get; set; } = PortCapacity.Multi;
        public PortCapacity OutCapacity { get; set; } = PortCapacity.Multi;
    }
}