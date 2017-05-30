using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ToggleGroupNode : ContainerNode
    {
        public ToggleGroupNode(JsonData jsonData) : base(jsonData) { }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            ToggleGroup toggleGroup = this.gameObject.AddComponent<ToggleGroup>();
            Toggle[] toggleArray = toggleGroup.GetComponentsInChildren<Toggle>();
            for(int i = 0; i < toggleArray.Length; i++)
            {
                toggleArray[i].group = toggleGroup;
            }
        }
    }
}
