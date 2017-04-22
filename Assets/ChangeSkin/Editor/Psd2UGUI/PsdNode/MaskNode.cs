using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class MaskNode : BaseNode
    {
        public const string MASK = "mask";

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);
            go.AddComponent<Image>();
            Mask mask = go.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            int length = Children.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Children[i].Build(go.transform);
            }

            AdjustPosition(go, parent);
        }
    }
}
