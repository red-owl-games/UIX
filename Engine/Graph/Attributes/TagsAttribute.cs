using System;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TagsAttribute : Attribute
    {
        public string[] Tags { get; }

        public TagsAttribute(params string[] tags)
        {
            Tags = tags;
        }
    }
}