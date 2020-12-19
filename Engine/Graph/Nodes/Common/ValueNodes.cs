namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common/Value")]
    public abstract class ValueNode<TValue> : Node
    {
        [ValueOut] public ValuePort Value;
        
        protected ValueNode(TValue defaultValue = default)
        {
            Value = new ValuePort<TValue>(this, defaultValue);
        }
    }
    
    public class StringValueNode : ValueNode<string>
    {
        public StringValueNode() : this(default) {}
        public StringValueNode(string defaultValue) : base(defaultValue) { }
    }
    
    public class CharValueNode : ValueNode<char>
    {
        public CharValueNode() : this(default) {}
        public CharValueNode(char defaultValue) : base(defaultValue) { }
    }

    public class BoolValueNode : ValueNode<bool>
    {
        public BoolValueNode() : this(default) {}
        public BoolValueNode(bool defaultValue) : base(defaultValue) { }
    }
    
    public class FloatValueNode : ValueNode<float>
    {
        public FloatValueNode() : this(default) {}
        public FloatValueNode(float defaultValue) : base(defaultValue) { }
    }
    
    public class DoubleValueNode : ValueNode<double>
    {
        public DoubleValueNode() : this(default) {}
        public DoubleValueNode(double defaultValue) : base(defaultValue) { }
    }
    
    public class ShortValueNode : ValueNode<short>
    {
        public ShortValueNode() : this(default) {}
        public ShortValueNode(short defaultValue) : base(defaultValue) { }
    }
    
    public class IntValueNode : ValueNode<int>
    {
        public IntValueNode() : this(default) {}
        public IntValueNode(int defaultValue) : base(defaultValue) { }
    }
    
    public class LongValueNode : ValueNode<long>
    {
        public LongValueNode() : this(default) {}
        public LongValueNode(long defaultValue) : base(defaultValue) { }
    }
    
    public class UShortValueNode : ValueNode<ushort>
    {
        public UShortValueNode() : this(default) {}
        public UShortValueNode(ushort defaultValue) : base(defaultValue) { }
    }
    
    public class UIntValueNode : ValueNode<uint>
    {
        public UIntValueNode() : this(default) {}
        public UIntValueNode(uint defaultValue) : base(defaultValue) { }
    }
    
    public class ULongValueNode : ValueNode<ulong>
    {
        public ULongValueNode() : this(default) {}
        public ULongValueNode(ulong defaultValue) : base(defaultValue) { }
    }
    
    public class SByteValueNode : ValueNode<sbyte>
    {
        public SByteValueNode() : this(default) {}
        public SByteValueNode(sbyte defaultValue) : base(defaultValue) { }
    }
    
    public class ByteValueNode : ValueNode<byte>
    {
        public ByteValueNode() : this(default) {}
        public ByteValueNode(byte defaultValue) : base(defaultValue) { }
    }
}