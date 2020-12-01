using System;
using PortView = UnityEditor.Experimental.GraphView.Port;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An input port exposed on a Node
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InputAttribute : Attribute
    {
        /// <summary>
        /// Display name of the input slot.
        ///
        /// If not supplied, this will default to the field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Can this input accept multiple outputs at once.
        /// </summary>
        public PortView.Capacity Capacity { get; set; }

        public InputAttribute(string name = null)
        {
            Name = name;
        }
    }
}