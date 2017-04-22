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
                    result = new TextNode();
                    break;
                case NodeType.IMAGE:
                    string nameStr = jsonData[NodeField.NAME].ToString().ToLower();
                    if(nameStr == NodeType.PLACEHOLDER)
                    {
                        result = new PlaceholderNode();
                    }
                    else
                    {
                        result = new ImageNode();
                    }
                    break;
                case NodeType.IMAGE_FOLDER:
                    result = new ImageFolderNode();
                    break;
                case NodeType.MASK:
                    result = new MaskNode();
                    break;
                case NodeType.BUTTON:
                    result = new ButtonNode();
                    break;
                case NodeType.SCROLL_VIEW:
                    result = new ScrollViewNode();
                    break;
                case NodeType.TOGGLE_GROUP:
                    result = new ToggleGroupNode();
                    break;
                case NodeType.TOGGLE:
                    result = new ToggleNode();
                    break;
                case NodeType.LIST:
                    result = new ListNode();
                    break;
                default:
                    result = new ContainerNode();
                    break;
            }
            result.Name = jsonData[NodeField.NAME].ToString();
            result.Width = (int)jsonData[NodeField.WIDTH];
            result.Height = (int)jsonData[NodeField.HEIGHT];
            result.X = (int)jsonData[NodeField.X];
            result.Y = (int)jsonData[NodeField.Y];
            return result;
        }
    }
}
