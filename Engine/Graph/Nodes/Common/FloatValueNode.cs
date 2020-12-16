using System;

namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class FloatValueNode : Node
    {
        [ValueOut] public ValuePort Value;

        public FloatValueNode() : this(0) {}

        public FloatValueNode(float defaultValue = 0)
        {
            Value = new ValuePort<float>(this, defaultValue);
        }
    }
}