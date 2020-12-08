using System;

namespace RedOwl.UIX.Engine
{
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