using LitJson;

namespace Psd2UGUI
{
    public class NodeFactory
    {
        public static BaseNode Create(JsonData jsonData)
        {
            BaseNode result;
            string typeStr = jsonData[NodeField.TYPE].ToString().ToLower();
            switch(typeStr)
            {
                case NodeType.TEXT:
                    result = new TextNode(jsonData);
                    break;
                case NodeType.IMAGE:
                    string nameStr = jsonData[NodeField.NAME].ToString().ToLower();
                    if(nameStr == NodeType.PLACEHOLDER)
                    {
                        result = new PlaceholderNode(jsonData);
                    }
                    else
                    {
                        result = new ImageNode(jsonData);
                    }
                    break;
                case NodeType.MASK:
                    result = new MaskNode(jsonData);
                    break;
                case NodeType.BUTTON:
                    result = new ButtonNode(jsonData);
                    break;
                case NodeType.ENTER_EXIT_BUTTON:
                    result = new EnterExitButtonNode(jsonData);
                    break;
                case NodeType.TOGGLE:
                    result = new ToggleNode(jsonData);
                    break;
                case NodeType.SCROLL_VIEW:
                    result = new ScrollViewNode(jsonData);
                    break;
                case NodeType.INPUT:
                    result = new InputNode(jsonData);
                    break;
                default:
                    result = new ContainerNode(jsonData);
                    break;
            }
            return result;
        }
    }
}
