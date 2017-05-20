using AssetManager;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using Tool;

namespace Psd2UGUI
{
    public class ImageNode : BaseNode
    {
        private string _psdName;
        private string _spriteName;
        private int _alpha = 255;
        private bool _isSlice = false;
        private bool _isAttach = false;

        public ImageNode(JsonData jsonData) : base(jsonData)
        {
            if(jsonData.ContainKey(NodeField.SLICE)) _isSlice = true;
            if(jsonData.ContainKey(NodeField.ATTACH)) _isAttach = true;
            if(jsonData.ContainKey(NodeField.BELONG_PSD)) _psdName = jsonData[NodeField.BELONG_PSD].ToString();
            if(jsonData.ContainKey(NodeField.ALPHA)) _alpha = int.Parse(jsonData[NodeField.ALPHA].ToString());
            _spriteName = jsonData[NodeField.NAME].ToString();
        }

        public override void Build(Transform parent)
        {
            if(string.IsNullOrEmpty(_psdName))
            {
                return;
            }
            if(_isAttach)
            {
                AttachImageToParent(parent);
                return;
            }
            GameObject go = CreateGameObject(parent);
            Image image = go.AddComponent<Image>();
            SetImage(image);
            AdjustPosition(go, parent);
        }

        private void AttachImageToParent(Transform parent)
        {
            Image image = parent.gameObject.AddComponent<Image>();
            SetImage(image);
        }

        private void SetImage(Image image)
        {
            image.sprite = AssetLoader.LoadSprite(_psdName, _spriteName);
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, _alpha / 255f);
            if(_isSlice)
            {
                image.type = Image.Type.Sliced;
            }
        }

    }
}
