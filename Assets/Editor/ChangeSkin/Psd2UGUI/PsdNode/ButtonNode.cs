using LitJson;
using UnityEngine;
using UnityEngine.UI;
using Tool;

namespace Psd2UGUI
{
    public class ButtonNode : ContainerNode
    {
        private bool _colorTint = false;
        private bool _addTransition = false;
        private float _scale;

        public ButtonNode(JsonData jsonData) : base(jsonData) 
        {
            if(jsonData.ContainKey(NodeField.COLOR_TINT))
            {
                _colorTint = true;
            }

            if (jsonData.ContainKey(NodeField.SCALE))
            {
                _addTransition = true;
                _scale = float.Parse(jsonData[NodeField.SCALE].ToString());
            }
        }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            Button button = AddComponent();
            button.transition = Selectable.Transition.None;
            if(_colorTint)
            {
                button.transition = Selectable.Transition.ColorTint;
                button.targetGraphic = this.gameObject.GetComponent<Image>();
            }
            if(_addTransition)
            {
                TransitionButton transitionButton = this.gameObject.AddComponent<TransitionButton>();
                transitionButton.scaleRate = _scale;
                TransformUtility.SetPivot(this.gameObject.GetComponent<RectTransform>(), TransformUtility.HalfVector2);
            }
        }

        protected virtual Button AddComponent()
        {
            return this.gameObject.AddComponent<Button>();
        }
    }
}