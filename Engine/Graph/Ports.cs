using System;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace RedOwl.UIX.Engine
{
    public enum PortDirection
    {
        Input,
        Output,
    }
    
    public enum PortCapacity
    {
        Single,
        Multi,
    }
    
    public class ValuePort
    {
        public PropertyInfo Info { get; }
        
        public string Name { get; }

        public Type Type { get; }

        public PortDirection Direction { get; }

        public PortCapacity Capacity { get; }

        public bool ShowElement { get; }
        
        public uint Hash { get; }

        public ValuePort(PropertyInfo info, PortDirection direction, PortCapacity capacity, string name = null)
        {
            Info = info;
            Name = name ?? info.Name;
            Type = info.PropertyType;
            Direction = direction;
            Capacity = capacity;
            ShowElement = info.GetGetMethod().IsFamily;
            //IsUsingFieldName = name != null; ???
            using (var algorithm = MD5.Create())
            {
                Hash = BitConverter.ToUInt32(algorithm.ComputeHash(Encoding.UTF8.GetBytes($"ValuePort_{Name}_{Direction}_{Type.FullName}")), 0);
            }
        }
    }

    public class FlowPort
    {
        private static readonly Type EnumerableType = typeof(IEnumerable);
        
        public MethodInfo MethodInfo { get; }
        public FieldInfo FieldInfo { get; }

        public string Name { get; }

        public PortDirection Direction { get; }
        
        public PortCapacity Capacity { get; }
        
        public bool IsAsync { get; }
        
        public uint Hash { get; }
        
        public FlowPort(MethodInfo info, PortCapacity capacity, string name = null)
        {
            MethodInfo = info;
            Name = name ?? info.Name;
            Direction = PortDirection.Input;
            Capacity = capacity;
            IsAsync = EnumerableType.IsAssignableFrom(info.ReturnType);
            using (var algorithm = MD5.Create())
            {
                Hash = BitConverter.ToUInt32(algorithm.ComputeHash(Encoding.UTF8.GetBytes($"FlowPort_{Name}_{Direction}_{IsAsync}")), 0);
            }
        }
        
        public FlowPort(FieldInfo info, PortCapacity capacity, string name = null)
        {
            FieldInfo = info;
            Name = name ?? info.Name;
            Direction = PortDirection.Output;
            Capacity = capacity;
            IsAsync = false;
            using (var algorithm = MD5.Create())
            {
                Hash = BitConverter.ToUInt32(algorithm.ComputeHash(Encoding.UTF8.GetBytes($"FlowPort_{Name}_{Direction}")), 0);
            }
        }
    }
}