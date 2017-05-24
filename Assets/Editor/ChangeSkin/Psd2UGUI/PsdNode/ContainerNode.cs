using LitJson;
using UnityEngine;
using Tool;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ContainerNode : BaseNode
    {
        private bool _addMask = false;
        private bool _addCanvas = false;
        public ContainerNode(JsonData jsonData) : base(jsonData) 
        {
            if(jsonData.ContainKey(NodeField.MASK))
            {
                _addMask = true;
            }

            if(jsonData.ContainKey(NodeField.CANVAS))
            {
                _addCanvas = true;
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
            }

            if(_addCanvas)
            {
                Canvas canvas = go.AddComponent<Canvas>();
                canvas.pixelPerfect = false;
                canvas.overridePixelPerfect = false;
                go.AddComponent<GraphicRaycaster>();
            }

            //重新计算坐标
            AdjustPosition(go, parent);
        }
    }
}
