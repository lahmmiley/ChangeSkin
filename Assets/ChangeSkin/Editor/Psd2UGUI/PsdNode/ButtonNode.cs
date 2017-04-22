using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ButtonNode : ContainerNode
    {
        public override void Build(Transform parent)
        {
            base.Build(parent);

            LButton button = this.gameObject.AddComponent<LButton>();
            button.targetGraphic = this.gameObject.transform.GetComponentInChildren<Image>();
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            Debug.LogError("OnClick!");
        }
    }
}
