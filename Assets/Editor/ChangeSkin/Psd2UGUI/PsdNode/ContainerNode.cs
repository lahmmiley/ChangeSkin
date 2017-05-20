using LitJson;
using UnityEngine;

namespace Psd2UGUI
{
    public class ContainerNode : BaseNode
    {
        public ContainerNode(JsonData jsonData) : base(jsonData) { }

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

            //重新计算坐标
            AdjustPosition(go, parent);
        }
    }
}
