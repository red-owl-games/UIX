using System;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public struct Slot
    {
        public string Node;
        public uint Port;
        
        private int? _id;

        public int Id
        {
            get
            {
                if (_id == null) _id = Node.GetHashCode() + Port.GetHashCode();
                return (int)_id;
            }
        }
    }
    
    [Serializable]
    public struct Connection
    {
        public Slot Output;
        public Slot Input;

        private int? _id;

        public int Id
        {
            get
            {
                if (_id == null) _id = Output.Id + Input.Id;
                return (int)_id;
            }
        }
    }
}