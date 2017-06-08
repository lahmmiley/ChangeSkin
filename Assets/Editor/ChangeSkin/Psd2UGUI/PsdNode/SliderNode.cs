using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class SliderNode : ContainerNode
    {
        public SliderNode(JsonData jsonData) : base(jsonData) {}

        public override void Build(Transform parent)
        {
            base.Build(parent);

            Slider slider = this.gameObject.AddComponent<Slider>();
            slider.transition = Selectable.Transition.None;
            slider.direction = Slider.Direction.LeftToRight;
            Transform transform = this.gameObject.transform;
            slider.fillRect = transform.Find("Fill Area/Fill").GetComponent<RectTransform>();
        }
    }
}
