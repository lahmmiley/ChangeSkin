using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ToggleGroupNode : ContainerNode
    {
        public override void Build(Transform parent)
        {
            base.Build(parent);

            ToggleGroup toggleGroup = this.gameObject.AddComponent<ToggleGroup>();
            for(int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                Transform tranform = this.gameObject.transform.GetChild(i);
                Toggle toggle = tranform.GetComponent<Toggle>();
                toggle.group = toggleGroup;
            }
        }
    }
}
