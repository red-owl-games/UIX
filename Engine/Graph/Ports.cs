using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace RedOwl.UIX.Engine
{
    /*
    public interface IPort
    {
        Orientation Orientation { get; }
        Port.Capacity Capacity { get; }
        Type FieldType { get; }
        string Name { get; }
    }

    public interface IValueInput : IPort { }

    public interface IValueOutput : IPort { }

    public interface IValueInOut : IValueInput, IValueOutput { }

    public class ValueIn<T> : IValueInput
    {
        public string Name { get; }
        public T Value { get; }
        public Orientation Orientation => Orientation.Horizontal;
        public Port.Capacity Capacity { get; }
        public Type FieldType => typeof(T);

        public ValueIn(string title, T defaultValue = default, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Name = ObjectNames.NicifyVariableName(title);
            Value = defaultValue;
            Capacity = capacity;
        }

        public static implicit operator T(ValueIn<T> self) => self.Value;
    }

    public class ValueOut<T> : IValueOutput
    {
        public string Name { get; }
        public T Value { get; set; }
        public Orientation Orientation => Orientation.Horizontal;
        public Port.Capacity Capacity { get; }
        public Type FieldType => typeof(T);
        
        public ValueOut(string title, T defaultValue = default, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Name = ObjectNames.NicifyVariableName(title);
            Value = defaultValue;
            Capacity = capacity;
        }

        public static implicit operator T(ValueOut<T> self) => self.Value;
    }
    
    public class ValueInOut<T> : IValueInOut
    {
        public string Name { get; }
        public T Value { get; }
        public Orientation Orientation => Orientation.Horizontal;
        public Port.Capacity Capacity { get; }
        public Type FieldType => typeof(T);

        public ValueInOut(string title, T defaultValue = default, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Name = ObjectNames.NicifyVariableName(title);
            Value = defaultValue;
            Capacity = capacity;
        }

        public static implicit operator T(ValueInOut<T> self) => self.Value;
    }
    */
}