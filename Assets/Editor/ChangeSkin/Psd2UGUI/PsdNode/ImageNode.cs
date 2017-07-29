using AssetManager;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using Tool;
using System;

namespace Psd2UGUI
{
    public class ImageNode : BaseNode
    {
        public enum Mirror
        {
            invalid = 0,
            up,
            right,
            down,
            left,
        };

        private string _psdName;
        private string _spriteName;
        private int _alpha = 255;
        private bool _isSlice = false;
        private bool _isPreserver = false;
        private bool _isAttach = false;
        private Mirror _mirror = Mirror.invalid;

        public ImageNode(JsonData jsonData) : base(jsonData)
        {
            if(jsonData.ContainKey(NodeField.SLICE)) _isSlice = true;
            if(jsonData.ContainKey(NodeField.PRESERVER)) _isPreserver = true;
            if(jsonData.ContainKey(NodeField.ATTACH)) _isAttach = true;
            if(jsonData.ContainKey(NodeField.BELONG_PSD)) _psdName = jsonData[NodeField.BELONG_PSD].ToString();
            if(jsonData.ContainKey(NodeField.ALPHA)) _alpha = int.Parse(jsonData[NodeField.ALPHA].ToString());
            if (jsonData.ContainKey(NodeField.MIRROR)) _mirror = (Mirror)Enum.Parse(typeof(Mirror), jsonData[NodeField.MIRROR].ToString());
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
            AdjustMirror(go);
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
            if(_isPreserver)
            {
                image.preserveAspect = true;
            }
        }

        private void AdjustMirror(GameObject go)
        {
            if (_mirror != Mirror.invalid)
            {
                RectTransform rect = go.GetComponent<RectTransform>();
                Mirror originMirror = ImageDataReader.Instance.GetMirror(Name);
                go.name = _mirror.ToString();
                int originMirroValue = (int)originMirror;
                int selfMirroValue = (int)_mirror;
                if(originMirroValue != selfMirroValue)
                {
                    TransformUtility.SetPivot(rect, TransformUtility.HalfVector2);
                    if((originMirroValue % 2) != (selfMirroValue % 2))
                    {
                        rect.Rotate(0, 0, 90);
                        SwapWidthHeight(rect);
                    }
                }
                //if((originMirror == Mirror.up && _mirror == Mirror.down)
                //    ||(originMirror == Mirror.down && _mirror == Mirror.up))
                //{
                //    rect.localScale = new Vector3(1, -1, 1);
                //    TransformUtility.SetPivot(rect, Vector2.right);
                //}
            }
        }

        private void SwapWidthHeight(RectTransform rect)
        {
            Vector2 sizeDelta = rect.sizeDelta;
            rect.sizeDelta = new Vector2(sizeDelta.y, sizeDelta.x);
        }

    }
}
