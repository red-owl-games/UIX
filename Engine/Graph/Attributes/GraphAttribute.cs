using System;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class GraphAttribute : Attribute
    {
        public string[] Tags { get; }

        public GraphAttribute(params string[] tags)
        {
            Tags = tags;
        }
    }
}