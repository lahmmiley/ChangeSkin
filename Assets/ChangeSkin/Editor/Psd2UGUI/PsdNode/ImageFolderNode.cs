using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using AssetManager;

namespace Psd2UGUI
{
    public class ImageFolderNode : BaseNode
    {
        private string _psdName;
        private bool _hasParam = false;

        public const string STATE_NORMAL = "normal";
        public const string STATE_OVER = "over";
        public const string STATE_DISABLE = "disable";

        protected Dictionary<string, string> stateDict;

        protected void SetState(JsonData jsonData)
        {
            stateDict = new Dictionary<string, string>();
            for(int i = 0; i < jsonData[NodeField.CHILDREN].Count; i++)
            {
                JsonData child = jsonData[NodeField.CHILDREN][i];
                string state = child[NodeField.NAME].ToString().ToLower();
                string name = child[NodeField.CHILDREN][0][NodeField.NAME].ToString();
                stateDict.Add(state, name);
            }
        }

        public override void ProcessStruct(JsonData jsonData)
        {
            for(int i = 0; i < jsonData[NodeField.CHILDREN].Count; i++)
            {
                JsonData child = jsonData[NodeField.CHILDREN][i];
                _psdName = child[NodeField.CHILDREN][0][NodeField.BELONG_PSD].ToString();
                if(child[NodeField.CHILDREN][0].Keys.Contains(NodeField.PARAM))
                {
                    _hasParam = true;
                }
                break;
            }
            SetState(jsonData);
        }

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);
            Image image = go.AddComponent<Image>();
            image.sprite = AssetLoader.LoadSprite(_psdName, stateDict[STATE_NORMAL]);
            if(_hasParam)
            {
                image.type = Image.Type.Sliced;
            }

            if(stateDict.Count > 1)
            {
                Selectable selectable = go.AddComponent<Selectable>();
                selectable.targetGraphic = image;
                selectable.transition = Selectable.Transition.SpriteSwap;

                SpriteState spriteState = new SpriteState();
                foreach(string state in stateDict.Keys)
                {
                    Sprite sprite = AssetLoader.LoadSprite(_psdName, stateDict[state]);
                    switch(state)
                    {
                        case STATE_NORMAL:
                            spriteState.highlightedSprite = sprite;
                            break;
                        case STATE_OVER:
                            spriteState.pressedSprite= sprite;
                            break;
                        case STATE_DISABLE:
                            spriteState.disabledSprite= sprite;
                            break;
                    }
                }
                selectable.spriteState = spriteState;//必须放在后面
            }

            AdjustPosition(go, parent);
        }
    }
}
