using System;

namespace RedOwl.UIX.Engine
{
    /// <summary>
    /// Define a Graph type and the nodes with a <c>[Tags]</c> attribute to include
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GraphAttribute : Attribute
    {
        public string[] Tags { get; set; }

        public GraphAttribute(params string[] tags)
        {
            Tags = tags;
        }
    }
}