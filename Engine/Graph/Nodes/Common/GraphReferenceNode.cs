namespace RedOwl.UIX.Engine
{
    [Node("Common", Path = "Common")]
    public class GraphReferenceNode : Node
    {
        public GraphAsset Graph;
        
        public override void Definition()
        {
            // TODO: Generate Dynamic Ports based on graph's PortNodes
        }
    }
}