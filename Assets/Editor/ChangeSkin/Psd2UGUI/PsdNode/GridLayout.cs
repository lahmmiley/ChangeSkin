using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class GridLayoutNode : ContainerNode
    {
        public GridLayoutNode(JsonData jsonData) : base(jsonData) {}

        public override void Build(Transform parent)
        {
            base.Build(parent);

            this.gameObject.AddComponent<GridLayoutGroup>();
        }
    }
}
