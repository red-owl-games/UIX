using System;
using System.Collections;

namespace RedOwl.UIX.Engine
{
    [Serializable]
    public class FlowPort : Port
    {
        private Func<IEnumerator> _callback;
        
        public FlowPort(INode node) : base(node) {}
        
        public override IEnumerator Execute() => _callback();

        public void SetCallback(Func<IEnumerator> callback) => _callback = callback;
    }
}