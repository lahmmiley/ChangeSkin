using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class CanvasNode : ContainerNode
    {
        private int _sortingOrder = 0;
        private bool _addCanvas = false;

        public CanvasNode(JsonData jsonData) : base(jsonData)
        {
            if(jsonData.ContainKey(NodeField.CANVAS))
            {
                _addCanvas = true;
                _sortingOrder = int.Parse(jsonData.ContainKey(NodeField.CANVAS).ToString());
            }
        }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            if(_addCanvas)
            {
                Canvas canvas = this.gameObject.AddComponent<Canvas>();
                canvas.pixelPerfect = false;
                canvas.overrideSorting = false;
                canvas.overridePixelPerfect = false;
                canvas.sortingOrder = _sortingOrder;
                GraphicRaycaster graphicRaycaster = this.gameObject.AddComponent<GraphicRaycaster>();
                graphicRaycaster.ignoreReversedGraphics = true;
                graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            }
        }
    }
}
