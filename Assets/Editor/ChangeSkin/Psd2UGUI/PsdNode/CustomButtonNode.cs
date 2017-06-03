using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class CustomButtonNode : ButtonNode
    {
        public CustomButtonNode(JsonData jsonData) : base(jsonData) {}

        protected override Button AddComponent()
        {
            return this.gameObject.AddComponent<CustomButton>();
        }
    }
}
