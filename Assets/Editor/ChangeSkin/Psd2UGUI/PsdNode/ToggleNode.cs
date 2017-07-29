using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ToggleNode : ContainerNode
    {
        public ToggleNode(JsonData jsonData) : base(jsonData) { }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            Toggle toggle = this.gameObject.AddComponent<Toggle>();
            toggle.transition = Selectable.Transition.ColorTint;
            toggle.targetGraphic = GetTargetGraphic();
            toggle.graphic = GetGraphic();
        }

        private Graphic GetTargetGraphic()
        {
            Transform transform = this.gameObject.transform;
            if(transform.Find("Background") != null)
            {
                Image imageBackground = transform.Find("Background").GetComponent<Image>();
                return imageBackground;
            }
            return transform.GetComponent<Image>();
        }

        private Graphic GetGraphic()
        {
            Transform transform = this.gameObject.transform;
            if(transform.Find("Background") != null)
            {
                Image imageCheckmark = transform.Find("Background/Checkmark").GetComponent<Image>();
                return imageCheckmark;
            }
            return transform.Find("Checkmark").GetComponent<Image>();
        }
    }
}
