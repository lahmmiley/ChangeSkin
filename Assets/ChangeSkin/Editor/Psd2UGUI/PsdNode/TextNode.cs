using LitJson;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class TextNode : BaseNode
    {
        private string _content = "未定义";
        private int _size;
        private Color _color;
        private TextAnchor _anchor = TextAnchor.UpperLeft;

        public override void ProcessStruct(JsonData jsonData)
        {
            if (jsonData.Keys.Contains(NodeField.SIZE))
            {
                _size = (int)jsonData[NodeField.SIZE];
            }

            if(jsonData.Keys.Contains(NodeField.TEXT))
            {
                _content = jsonData[NodeField.TEXT].ToString();
            }

            if(jsonData.Keys.Contains(NodeField.COLOR))
            {
                _color = jsonData[NodeField.COLOR].ToString().ToColor();
            }

            if(jsonData.Keys.Contains(NodeField.PARAM))
            {
                string param = jsonData[NodeField.PARAM].ToString();
                if(param.IndexOf("Left") != -1)
                {
                    _anchor = TextAnchor.UpperLeft;
                }
                else if(param.IndexOf("Center") != -1)
                {
                    _anchor = TextAnchor.UpperCenter;
                }
                else if(param.IndexOf("Right") != -1)
                {
                    _anchor = TextAnchor.UpperRight;
                }
            }
        }

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);
            Text text = go.AddComponent<Text>();
            text.font = Resources.Load("Font/arial") as Font;
            text.fontSize = _size;
            text.text = _content;
            text.color = _color;
            //TODO 如果只有一行，就用overflow
            //如果有多行，缺少的宽度就没有影响了
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = _anchor;

            AdjustPosition(go, parent);
        }
    }
}
