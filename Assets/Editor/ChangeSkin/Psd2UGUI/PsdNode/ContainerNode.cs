using LitJson;
using UnityEngine;
using Tool;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ContainerNode : BaseNode
    {
        private bool _addMask = false;
        private bool _addSizeFitter = false;
        private bool _addHorizontalLayout = false;
        private bool _addVerticalLayout = false;
        private bool _addElement = false;

        public ContainerNode(JsonData jsonData) : base(jsonData) 
        {
            if(jsonData.ContainKey(NodeField.MASK))
            {
                _addMask = true;
            }
            if(jsonData.ContainKey(NodeField.SIZE_FITTER))
            {
                _addSizeFitter = true;
            }
            if(jsonData.ContainKey(NodeField.HORIZONTAL_LAYOUT))
            {
                _addHorizontalLayout = true;
            }
            if(jsonData.ContainKey(NodeField.VERTICAL_LAYOUT))
            {
                _addVerticalLayout = true;
            }
            if(jsonData.ContainKey(NodeField.ELEMENT))
            {
                _addElement = true;
            }
        }

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);
            this.gameObject = go;
            if(Children != null)
            {
                int length = Children.Length;
                for (int i = length - 1; i >= 0; i--)
                {
                    Children[i].Build(go.transform);
                }
            }

            if(_addMask)
            {
                Mask mask = go.AddComponent<Mask>();
                mask.showMaskGraphic = false;

                Image image = go.GetComponent<Image>();
                Color color = image.color;
                image.color = new Color(color.r, color.g, color.b, 1);
            }

            if(_addSizeFitter)
            {
                go.AddComponent<ContentSizeFitter>();
            }

            if(_addHorizontalLayout)
            {
                go.AddComponent<HorizontalLayoutGroup>();
            }

            if(_addVerticalLayout)
            {
                go.AddComponent<VerticalLayoutGroup>();
            }

            if(_addElement)
            {
                go.AddComponent<LayoutElement>();
            }

            //重新计算坐标
            AdjustPosition(go, parent);
        }
    }
}
