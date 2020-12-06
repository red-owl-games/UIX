#if ODIN_INSPECTOR
using System.Collections.Generic;
using RedOwl.UIX.Engine;
using Sirenix.OdinInspector.Editor;

namespace RedOwl.UIX.Editor
{
    public class UIXGraphNodePropertyProcessor<T> : OdinPropertyProcessor<T> where T : INode
    {
        public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
        {
            for (int i = propertyInfos.Count - 1; i >= 0; i--)
            {
                var info = propertyInfos[i];
                if (info.GetAttribute<InputAttribute>() != null) propertyInfos.RemoveAt(i);
                if (info.GetAttribute<OutputAttribute>() != null) propertyInfos.RemoveAt(i);
                if (info.GetAttribute<InOutAttribute>() != null) propertyInfos.RemoveAt(i);
            }
        }
    }
}

#endif