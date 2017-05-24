using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class EnterExitButtonNode : ButtonNode
    {
        public EnterExitButtonNode(JsonData jsonData) : base(jsonData) {}

        protected override Button AddComponent()
        {
            return this.gameObject.AddComponent<CustomEnterExsitButton>();
        }
    }
}
