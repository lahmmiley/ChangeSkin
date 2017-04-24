using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class PlaceholderNode : BaseNode
    {
        private bool _isAttach = false;
        public override void ProcessStruct(JsonData jsonData)
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
                Image image = parent.gameObject.AddComponent<Image>();
            }
        }
    }
}
