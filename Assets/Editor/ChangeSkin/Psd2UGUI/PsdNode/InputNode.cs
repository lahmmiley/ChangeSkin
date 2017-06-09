using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class InputNode : ContainerNode
    {
        public InputNode(JsonData jsonData) : base(jsonData) {}

        public override void Build(Transform parent)
        {
            base.Build(parent);

            InputField inputField = this.gameObject.AddComponent<InputField>();
            inputField.transition = Selectable.Transition.None;
            inputField.textComponent = GetTextComponent();
            inputField.placeholder = GetPlaceholder();
        }

        private Text GetTextComponent()
        {
            Transform transform = this.gameObject.transform;
            return transform.Find("Text").GetComponent<Text>();
        }

        private Graphic GetPlaceholder()
        {
            Transform transform = this.gameObject.transform;
            Transform placeholder = transform.Find("Placeholder");
            if(placeholder != null)
            {
                return placeholder.GetComponent<Text>();
            }
            return null;
        }
    }
}
