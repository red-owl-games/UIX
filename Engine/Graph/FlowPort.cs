using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public class FlowPort : Port
    {
        [SerializeField]
        private string callbackName;

        public FlowPort(INode node) : base(node)
        {
            callbackName = null;
        }
        
        public FlowPort(INode node, string callback) : base(node)
        {
            callbackName = callback;
        }
    }
}