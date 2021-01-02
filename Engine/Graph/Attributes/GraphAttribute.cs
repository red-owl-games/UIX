using System;
using UnityEngine.Scripting;

namespace RedOwl.UIX.Engine
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class GraphAttribute : PreserveAttribute
    {
        public string[] Tags { get; }

        public GraphAttribute(params string[] tags)
        {
            Tags = tags;
        }
    }
}