using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ButtonNode : ContainerNode
    {
        public ButtonNode(JsonData jsonData) : base(jsonData) { }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            Button button = this.gameObject.AddComponent<Button>();
            button.targetGraphic = this.gameObject.transform.GetComponentInChildren<Image>();
            button.transition = Selectable.Transition.ColorTint;
            button.onClick.AddListener(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            Debug.LogError("OnClick!");
        }
    }
}
