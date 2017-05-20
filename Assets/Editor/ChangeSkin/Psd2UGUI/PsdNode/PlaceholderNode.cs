using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class PlaceholderNode : BaseNode
    {
        private bool _isAttach = false;
        public PlaceholderNode(JsonData jsonData) : base(jsonData)
        {
            if(jsonData.Keys.Contains(NodeField.ATTACH))
            {
                _isAttach = true;
            }
        }

        public override void Build(Transform parent)
        {
            if(_isAttach)
            {
                parent.gameObject.AddComponent<Image>();
            }
        }
    }
}
