using LitJson;
using UnityEngine;
using Tool;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ContainerNode : BaseNode
    {
        private bool _addMask = false;

        public ContainerNode(JsonData jsonData) : base(jsonData) 
        {
            if(jsonData.ContainKey(NodeField.MASK))
            {
                _addMask = true;
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

            //重新计算坐标
            AdjustPosition(go, parent);
        }
    }
}
