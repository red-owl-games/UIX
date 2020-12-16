using System.Collections;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Node("Debug", Path = "Debug")]
    public class KitchenSinkNode : Node
    {
        // Flow
        [FlowIn] public FlowPort Enter;
        [FlowIn] public FlowPort Passthrough;

        [FlowOut] public FlowPort Start;
        [FlowOut] public FlowPort Changed;
        [FlowOut] public FlowPort Complete;
        
        // Primatives
        
        [ValueInOut] public ValuePort String;
        [ValueInOut] public ValuePort Char;
        [ValueInOut] public ValuePort Bool;
        [ValueInOut] public ValuePort Float;
        [ValueInOut] public ValuePort Double;
        // [ValueInOut] public ValuePort Decimal; // Does Not Serialize
        [ValueInOut] public ValuePort Short;
        [ValueInOut] public ValuePort Int;
        [ValueInOut] public ValuePort Long;
        [ValueInOut] public ValuePort UShort;
        [ValueInOut] public ValuePort UInt;
        [ValueInOut] public ValuePort ULong;
        [ValueInOut] public ValuePort SByte;
        [ValueInOut] public ValuePort Byte;
        
        // Array
        // [ValueInOut] public ValuePort StringArray; // Caused a Crash on Recompile
        // [ValueInOut] public ValuePort FloatArray; // Caused a Crash on Recompile
        //
        // // List
        // [ValueInOut] public ValuePort StringList; // Caused a Crash on Recompile
        // [ValueInOut] public ValuePort FloatList; // Caused a Crash on Recompile
        
        // Complex Types
        // [ValueInOut] public ValuePort Uri; // Does Not Serialize
        // [ValueInOut] public ValuePort Guid; // Does Not Serialize
        // [ValueInOut] public ValuePort DateTime; // Does Not Serialize
        // [ValueInOut] public ValuePort DateTimeOffset; // Does Not Serialize
        // [ValueInOut] public ValuePort TimeSpan; // Does Not Serialize
        
        // Unity Types
        [ValueInOut] public ValuePort Color;
        [ValueInOut] public ValuePort Color32;
        [ValueInOut] public ValuePort Vector2;
        [ValueInOut] public ValuePort Vector3;
        [ValueInOut] public ValuePort Vector4;
        [ValueInOut] public ValuePort Vector2Int;
        [ValueInOut] public ValuePort Vector3Int;
        [ValueInOut] public ValuePort Quaternion;
        [ValueInOut] public ValuePort Bounds;
        [ValueInOut] public ValuePort BoundsInt;
        [ValueInOut] public ValuePort Rect;
        [ValueInOut] public ValuePort RectInt;
        // [ValueInOut] public ValuePort AnimationCurve; // Caused a Crash on Recompile
        // [ValueInOut] public ValuePort Gradient; // Caused a Crash on Recompile
        // [ValueInOut] public ValuePort LayerMask; // Caused a Crash on Recompile
        // [ValueInOut] public ValuePort Texture; // Does Not Serialize
        // [ValueInOut] public ValuePort Texture2D; // Does Not Serialize
        // [ValueInOut] public ValuePort Texture3D; // Does Not Serialize
        // [ValueInOut] public ValuePort RenderTexture; // Does Not Serialize
        
        // Unity Mathamatics
        
        // Field
        public string Field = "Test";
        
        public KitchenSinkNode()
        {
            Enter = new FlowPort(this, nameof(OnEnter));
            Passthrough = new FlowPort(this, nameof(OnPassthrough));
            
            Start = new FlowPort(this);
            Changed = new FlowPort(this);
            Complete = new FlowPort(this);
            
            String = new ValuePort<string>(this);
            Char = new ValuePort<char>(this);
            Bool = new ValuePort<bool>(this);
            Float = new ValuePort<float>(this);
            Double = new ValuePort<double>(this);
            // Decimal = new ValuePort<decimal>(this);
            Short = new ValuePort<short>(this);
            Int = new ValuePort<int>(this);
            Long = new ValuePort<long>(this);
            UShort = new ValuePort<ushort>(this);
            UInt = new ValuePort<uint>(this);
            ULong = new ValuePort<ulong>(this);
            SByte = new ValuePort<sbyte>(this);
            Byte = new ValuePort<byte>(this);

            // StringArray = new ValuePort<string[]>(this, new []{"Hello", "World"});
            // FloatArray = new ValuePort<float[]>(this, new []{1.5f, 0.5f});
            //
            // StringList = new ValuePort<List<string>>(this, new List<string> {"Hello", "World"});
            // FloatList = new ValuePort<List<float>>(this, new List<float> {1.5f, 0.5f});
            
            // Uri = new ValuePort<Uri>(this);
            // Guid = new ValuePort<Guid>(this);
            // DateTime = new ValuePort<DateTime>(this);
            // DateTimeOffset = new ValuePort<DateTimeOffset>(this);
            // TimeSpan = new ValuePort<TimeSpan>(this);
            
            Color = new ValuePort<Color>(this);
            Color32 = new ValuePort<Color32>(this);
            Vector2 = new ValuePort<Vector2>(this);
            Vector3 = new ValuePort<Vector3>(this);
            Vector4 = new ValuePort<Vector4>(this);
            Vector2Int = new ValuePort<Vector2Int>(this);
            Vector3Int = new ValuePort<Vector3Int>(this);
            Quaternion = new ValuePort<Quaternion>(this);
            Bounds = new ValuePort<Bounds>(this);
            BoundsInt = new ValuePort<BoundsInt>(this);
            Rect = new ValuePort<Bounds>(this);
            RectInt = new ValuePort<RectInt>(this);
            // AnimationCurve = new ValuePort<AnimationCurve>(this);
            // Gradient = new ValuePort<Gradient>(this);
            // LayerMask = new ValuePort<LayerMask>(this);
            // Texture = new ValuePort<Texture>(this);
            // Texture2D = new ValuePort<Texture2D>(this);
            // Texture3D = new ValuePort<Texture3D>(this);
            // RenderTexture = new ValuePort<RenderTexture>(this);
            
            // Unity Mathamatics
        }
        
        private IEnumerable OnEnter(IFlow flow)
        {
            yield return Start;

            for (int i = 0; i < 5; i++)
            {
                yield return Changed;
                yield return new WaitForSeconds(0.5f);
            }

            yield return Complete;
        }

        private IEnumerable OnPassthrough(IFlow flow)
        {
            yield return Complete;
        }
    }
}
