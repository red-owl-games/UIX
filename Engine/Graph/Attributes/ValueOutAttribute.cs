using System;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An output port exposed on a Node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValueOutAttribute : Attribute
    {
        /// <summary>
        /// Display name of the output slot.
        ///
        /// If not supplied, this will default to the field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Can this output go to multiple inputs at once.
        /// </summary>
        public PortCapacity Capacity { get; set; } = PortCapacity.Multi;

        /// <summary>
        /// If defined as a class attribute, this is the output type.
        ///
        /// When defined on a field, the output will automatically be inferred by the field.
        /// </summary>
        public Type Type { get; set; }

        public ValueOutAttribute(string name = null, Type type = null)
        {
            Name = name;
            Type = type;
        }
    }
}