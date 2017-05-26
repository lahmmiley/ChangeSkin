using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class DefaultScrollBarNode : BaseNode
    {
        public DefaultScrollBarNode(JsonData jsonData) : base(jsonData)
        {
        }

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);

            GameObject handle = CreateHandle();

            Scrollbar scrollbar = go.AddComponent<Scrollbar>();
        }

        private GameObject CreateHandle()
        {
            GameObject go = new GameObject();
            go.name = "Handle";
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = Vector2.up;
            rect.anchorMax = Vector2.up;
            rect.anchorMin = Vector2.up;
        }
    }
}
