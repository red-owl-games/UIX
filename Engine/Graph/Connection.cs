using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public class PortCollection : BetterCollection<PortId> {}

    [Serializable]
    public class ConnectionsGraph : BetterDictionary<PortId, PortCollection>
    {
        public void Connect(PortId output, PortId input)
        {
            if (TryGetValue(output, out var collection))
            {
                collection.Add(input);
                return;
            }

            collection = new PortCollection{input};
            Add(output, collection);
        }

        public void Disconnect(PortId output, PortId input)
        {
            if (TryGetValue(output, out var collection))
            {
                collection.Remove(input);
                this[output] = collection;
            }
        }
    }
}
