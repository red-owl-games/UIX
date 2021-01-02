// using System;
// using System.Collections;
//
// namespace RedOwl.UIX.Engine
// {
//     public class Port
//     {
//         public string id { get; private set; }
//         public string title { get; private set; }
//
//         public PortDirection direction { get; private set; } = PortDirection.None;
//         public PortCapacity inCapacity { get; private set; } = PortCapacity.Multi;
//         public PortCapacity outCapacity { get; private set; } = PortCapacity.Multi;
//
//         public bool hasCallback { get; private set; } = false;
//         public Action<IFlow> simpleCallback { get; private set; } = null;
//         public Func<IFlow, IFlowPort> syncCallback { get; private set; } = null;
//         public Func<IFlow, IEnumerator> asyncCallback { get; private set; } = null;
//         
//         public Port(string id = null, string title = null)
//         {
//             this.id = id;
//             this.title = string.IsNullOrEmpty(title) ? id : title;
//         }
//
//         private void CreateHash()
//         {
//             // TODO: figure out the best way to do this
//             id = $"{title}.{direction}.{inCapacity}.{outCapacity}.{hasCallback}";
//         }
//
//         public Port Title(string value)
//         {
//             title = value;
//             return this;
//         }
//         
//         public Port Direction(PortDirection value)
//         {
//             direction = value;
//             return this;
//         }
//
//         public Port In(PortCapacity capacity = PortCapacity.Multi)
//         {
//             direction |= PortDirection.Input;
//             inCapacity = capacity;
//             return this;
//         }
//
//         public Port Out(PortCapacity capacity = PortCapacity.Multi)
//         {
//             direction |= PortDirection.Output;
//             outCapacity = capacity;
//             return this;
//         }
//         
//         public Port InOut(PortCapacity @in = PortCapacity.Multi, PortCapacity @out = PortCapacity.Multi)
//         {
//             direction |= PortDirection.Input;
//             direction |= PortDirection.Output;
//             inCapacity = @in;
//             outCapacity = @out;
//             return this;
//         }
//
//         public Port Capacity(PortCapacity capacity)
//         {
//             inCapacity = outCapacity = capacity;
//             return this;
//         }
//
//         public Port Single()
//         {
//             inCapacity = outCapacity = PortCapacity.Single;
//             return this;
//         }
//
//         public Port Multi()
//         {
//             inCapacity = outCapacity = PortCapacity.Multi;
//             return this;
//         }
//         
//         public Port Callback(Action<IFlow> callback)
//         {
//             hasCallback = true;
//             simpleCallback = callback;
//             return this;
//         }
//         
//         public Port Callback(Func<IFlow, IFlowPort> callback)
//         {
//             hasCallback = true;
//             syncCallback = callback;
//             return this;
//         }
//         
//         public Port Callback(Func<IFlow, IEnumerator> callback)
//         {
//             hasCallback = true;
//             asyncCallback = callback;
//             return this;
//         }
//
//         public FlowPort BuildFlowPort()
//         {
//             return null;
//         }
//
//         public ValuePort<T> Value<T>(T defaultValue = default)
//         {
//             if (string.IsNullOrEmpty(id)) CreateHash();
//             return null;
//         }
//
//         public static implicit operator FlowPort(Port builder)
//         {
//             return builder.BuildFlowPort();
//         }
//     }
// }