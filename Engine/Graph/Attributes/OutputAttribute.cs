using System;
using PortView = UnityEditor.Experimental.GraphView.Port;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An output port exposed on a Node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OutputAttribute : Attribute
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
        public PortView.Capacity Capacity { get; set; }

        /// <summary>
        /// If defined as a class attribute, this is the output type.
        ///
        /// When defined on a field, the output will automatically be inferred by the field.
        /// </summary>
        public Type Type { get; set; }

        public OutputAttribute(string name = null, Type type = null)
        {
            Name = name;
            Type = type;
        }
    }
}