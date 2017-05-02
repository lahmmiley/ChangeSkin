using LitJson;
using System.Collections.Generic;
using Tool;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class TextNode : BaseNode
    {
        private static Dictionary<int, int> psdFontSizeChanger = new Dictionary<int, int>();

        public enum Orientation
        {
            horizontal,
            vertical,
        }

        static TextNode()
        {
            psdFontSizeChanger[10] = 8;
            psdFontSizeChanger[11] = 8;
            psdFontSizeChanger[12] = 9;
            psdFontSizeChanger[13] = 10;
            psdFontSizeChanger[14] = 11;
            psdFontSizeChanger[15] = 11;
            psdFontSizeChanger[16] = 12;
            psdFontSizeChanger[17] = 13;
            psdFontSizeChanger[18] = 14;
            psdFontSizeChanger[19] = 15;
            psdFontSizeChanger[20] = 15;

            psdFontSizeChanger[21] = 15;
            psdFontSizeChanger[22] = 16;
            psdFontSizeChanger[23] = 17;
            psdFontSizeChanger[24] = 18;
            psdFontSizeChanger[25] = 19;
        }

        private string _content = "未定义";
        private int _size;
        private Color _color;
        private TextAnchor _anchor = TextAnchor.UpperLeft;
        private bool _isOneLine = false;
        private Orientation _orientation;
        private float _lineSpacing = 1;

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

            if (jsonData.Keys.Contains(NodeField.ONE_LINE))
            {
                _isOneLine = ((int)jsonData[NodeField.ONE_LINE] == 1) ? true : false;
            }

            if (jsonData.Keys.Contains(NodeField.Orientation))
            {
                _orientation = jsonData[NodeField.Orientation].ToString() == "horizontal" ? Orientation.horizontal : Orientation.vertical;
            }

            if (jsonData.ContainKey(NodeField.LineSpacing))
            {
                _lineSpacing = float.Parse(jsonData[NodeField.LineSpacing].ToString());
            }

            if(jsonData.ContainKey(NodeField.PARAM))
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
            text.font = AssetDatabase.LoadAssetAtPath("Assets/Font/wqy.ttf", typeof(Font)) as Font;
            if(!psdFontSizeChanger.ContainsKey(_size))
            {
                Debug.LogError(_size);
                _size = 24;
            }
            text.fontSize = psdFontSizeChanger[_size];
            text.text = _content;
            text.color = _color;
            if(_isOneLine)
            {
                if(_orientation == Orientation.horizontal)
                {
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;
                }
                else
                {
                    text.verticalOverflow = VerticalWrapMode.Overflow;
                }
            }
            text.lineSpacing = _lineSpacing;
            text.alignment = _anchor;

            AdjustPosition(go, parent);
        }
    }
}
