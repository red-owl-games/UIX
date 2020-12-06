using System;
using PortView = UnityEditor.Experimental.GraphView.Port;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// An input & output port exposed on a Node
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InOutAttribute : Attribute
    {
        public string InName { get; set; }
        public string OutName { get; set; }

        public PortView.Capacity InCapacity { get; set; }
        public PortView.Capacity OutCapacity { get; set; }

        public InOutAttribute(string inName = null, string outName = null)
        {
            InName = inName;
            OutName = outName;
        }
    }
}