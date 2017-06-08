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
        public DefaultScrollBarNode(JsonData jsonData) : base(jsonData) {}

        public override void Build(Transform parent)
        {
            GameObject go = CreateGameObject(parent);

            GameObject handle = CreateHandle();
            GameObject slidingArea = CreateSlidingArea();
            handle.transform.SetParent(slidingArea.transform);
            slidingArea.transform.SetParent(go.transform);
            Scrollbar scrollbar = go.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.LeftToRight;
            scrollbar.handleRect = handle.GetComponent<RectTransform>();
            scrollbar.transition = Selectable.Transition.None;
        }

        private GameObject CreateHandle()
        {
            GameObject goHandle = CreateGameObject("Handle", 0, 0, 0, 0);
            Image image = goHandle.AddComponent<Image>();
            return goHandle;
        }

        private GameObject CreateSlidingArea()
        {
            GameObject goSlidingArea = CreateGameObject("Sliding Area", 0, 0, 0, 0);
            return goSlidingArea;
        }
    }
}
