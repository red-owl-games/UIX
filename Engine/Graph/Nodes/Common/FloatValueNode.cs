namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class FloatValueNode : Node
    {
        [ValueOut] public float value { get; }
    }
}