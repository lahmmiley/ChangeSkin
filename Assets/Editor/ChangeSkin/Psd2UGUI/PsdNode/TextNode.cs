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
        public enum Orientation
        {
            horizontal,
            vertical,
        }

        private string _content = "未定义";
        private int _size;
        private Color _color;
        private TextAnchor _anchor = TextAnchor.UpperLeft;
        private bool _isOneLine = false;
        private Orientation _orientation;
        private float _lineSpacing = 1;
        private bool _isAttach = false;

        private bool _stroke = false;
        private int _strokeSize;
        private int _strokeAlpha;
        private Color _strokeColor;

        private bool _dropShadow = false;
        private int _dropShadowAlpha;
        private int _dropShadowAngle;
        private int _dropShadowDistance;
        private Color _dropShadowColor;

        public TextNode(JsonData jsonData) : base(jsonData)
        {
            _size = (int)jsonData[NodeField.SIZE];
            _content = jsonData[NodeField.TEXT].ToString();
            _color = jsonData[NodeField.COLOR].ToString().ToColor();
            _isOneLine = ((int)jsonData[NodeField.ONE_LINE] == 1) ? true : false;
            _orientation = jsonData[NodeField.ORIENTATION].ToString() == "horizontal" ? Orientation.horizontal : Orientation.vertical;
            if (jsonData.ContainKey(NodeField.ATTACH))
            {
                 _isAttach = true;
            }
            if(jsonData.ContainKey(NodeField.ALIGN))
            {
                _anchor = GetAlign(jsonData[NodeField.ALIGN].ToString());
            }

            if (jsonData.ContainKey(NodeField.LINE_SPACING))
            {
                _lineSpacing = float.Parse(jsonData[NodeField.LINE_SPACING].ToString());
            }

            if (jsonData.ContainKey(NodeField.STROKE_SIZE))
            {
                _stroke = true;
                _strokeSize = int.Parse(jsonData[NodeField.STROKE_SIZE].ToString());
                _strokeAlpha = int.Parse(jsonData[NodeField.STROKE_ALPHA].ToString());
                _strokeColor = jsonData[NodeField.STROKE_COLOR].ToString().ToColor();
            }

            if (jsonData.ContainKey(NodeField.DROP_SHADOW_ALPHA))
            {
                _dropShadow = true;
                _dropShadowAlpha = int.Parse(jsonData[NodeField.DROP_SHADOW_ALPHA].ToString());
                _dropShadowAngle = int.Parse(jsonData[NodeField.DROP_SHADOW_ANGLE].ToString());
                _dropShadowDistance = int.Parse(jsonData[NodeField.DROP_SHADOW_DISTANCE].ToString());
                _dropShadowColor = jsonData[NodeField.DROP_SHADOW_COLOR].ToString().ToColor();
            }
        }

        private TextAnchor GetAlign(string param)
        {
            switch(param)
            {
                case "lowercenter":
                    return TextAnchor.LowerCenter;
                case "lowerleft":
                    return TextAnchor.LowerLeft;
                case "lowerright":
                    return TextAnchor.LowerRight;
                case "middlecenter":
                    return TextAnchor.MiddleCenter;
                case "middleleft":
                    return TextAnchor.MiddleLeft;
                case "middleright":
                    return TextAnchor.MiddleRight;
                case "uppercenter":
                    return TextAnchor.UpperCenter;
                case "upperleft":
                    return TextAnchor.UpperLeft;
                case "upperright":
                    return TextAnchor.UpperRight;
                default:
                    return TextAnchor.UpperLeft;
            }
        }

        public override void Build(Transform parent)
        {
            if(_isAttach)
            {
                AttachTextToParent(parent);
                return;
            }

            GameObject go = CreateGameObject(parent);
            Text text = go.AddComponent<Text>();
            SetText(go, text);
            AdjustPosition(go, parent);
        }

        private void SetText(GameObject go, Text text)
        {
            text.font = AssetDatabase.LoadAssetAtPath("Assets/Font/wqy.ttf", typeof(Font)) as Font;
            text.fontSize = _size;
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
            BuildShadow(go);
            BuildOutLine(go);
        }

        private void AttachTextToParent(Transform parent)
        {
            GameObject goParent = parent.gameObject;
            Text text = goParent.AddComponent<Text>();
            SetText(goParent, text);
        }

        //投影
        private void BuildShadow(GameObject go)
        {
            if(_dropShadow)
            {
                Shadow shadow = go.AddComponent<Shadow>();
                shadow.effectColor = new Color(_dropShadowColor.r, _dropShadowColor.g, _dropShadowColor.b, _dropShadowAlpha * 2.55f);
                float x = -Mathf.Cos(Mathf.Deg2Rad * _dropShadowAngle) * _dropShadowDistance;
                float y = -Mathf.Sin(Mathf.Deg2Rad * _dropShadowAngle) * _dropShadowDistance;
                shadow.effectDistance = new Vector2(x, y);
            }
        }

        //描边
        private void BuildOutLine(GameObject go)
        {
            if(_stroke)
            {
                Outline outLine = go.AddComponent<Outline>();
                outLine.useGraphicAlpha = false;
                outLine.effectDistance = new Vector2(_strokeSize, -_strokeSize);
                outLine.effectColor = new Color(_strokeColor.r, _strokeColor.g, _strokeColor.b, _strokeAlpha * 2.55f);
            }
        }
    }
}
