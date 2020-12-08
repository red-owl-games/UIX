using System;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An input & output port exposed on a Node
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueInOutAttribute : Attribute
    {
        public string InName { get; set; }
        public string OutName { get; set; }

        public PortCapacity InCapacity { get; set; } = PortCapacity.Multi;
        public PortCapacity OutCapacity { get; set; } = PortCapacity.Multi;

        public ValueInOutAttribute(string inName = null, string outName = null)
        {
            InName = inName;
            OutName = outName;
        }
    }
}