using AssetManager;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ImageNode : BaseNode
    {
        private string _psdName;
        private string _spriteName;
        private bool _hasParam = false;
        private bool _isAttach = false;

        //TODO 叫ProcessStruct不是很好，感觉只是处理结构
        public override void ProcessStruct(JsonData jsonData)
        {
            _psdName = jsonData[NodeField.BELONG_PSD].ToString();
            if(jsonData.Keys.Contains(NodeField.PARAM))
            {
                _hasParam = true;
            }
            if(jsonData.Keys.Contains(NodeField.ATTACH))
            {
                _isAttach = true;
            }
            _spriteName = jsonData[NodeField.NAME].ToString();
        }

        public override void Build(Transform parent)
        {
            if(_isAttach)
            {
                AttachImageToParent(parent);
                return;
            }
            GameObject go = CreateGameObject(parent);
            Image image = go.AddComponent<Image>();
            image.sprite = AssetLoader.LoadSprite(_psdName, _spriteName);
            if(_hasParam)
            {
                image.type = Image.Type.Sliced;
            }
            
            AdjustPosition(go, parent);
        }

        private void AttachImageToParent(Transform parent)
        {
            Image image = parent.gameObject.AddComponent<Image>();
            image.sprite = AssetLoader.LoadSprite(_psdName, _spriteName);
            if(_hasParam)
            {
                image.type = Image.Type.Sliced;
            }
        }
    }
}
