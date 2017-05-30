using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tool;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class ScrollRectNode : ContainerNode
    {
        private bool _horizontal = false;
        private bool _vertical = false;

        public ScrollRectNode(JsonData jsonData) : base(jsonData)
        {
            if (jsonData.ContainKey(NodeField.DIRECTION))
            {
                string direction = jsonData[NodeField.DIRECTION].ToString();
                if (direction == "horizontal")
                {
                    _horizontal = true;
                }
                else if (direction == "vertical")
                {
                    _vertical = true;
                }
            }
        }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            ScrollRect scrollRect = this.gameObject.AddComponent<ScrollRect>();
            scrollRect.horizontal = _horizontal;
            scrollRect.vertical = _vertical;

            Transform transformContainer = this.gameObject.transform.Find("Container");
            scrollRect.content = transformContainer.GetComponent<RectTransform>();

            Transform transformScrollbar = this.gameObject.transform.Find("Scrollbar");
            if(_horizontal)
            {
                scrollRect.horizontalScrollbar = transformScrollbar.GetComponent<Scrollbar>();
            }
            if(_vertical)
            {
                scrollRect.verticalScrollbar = transformScrollbar.GetComponent<Scrollbar>();
            }
        }
    }
}
