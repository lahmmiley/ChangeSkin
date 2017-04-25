using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Psd2UGUI
{
    public class Test
    {
        private static RectTransform rectImage;
        private static RectTransform rectChild;

        private static RectTransform rectCreate;
        private static RectTransform rectOldChild;

        [MenuItem("Psd2UGUI/Test")]
        private static void Init()
        {
            GameObject goImage = GameObject.Find("Canvas/Old/Image");
            rectImage = goImage.GetComponent<RectTransform>();

            //GameObject goChild = GameObject.Find("Canvas/Old/Image/child");
            //rectChild = goChild.GetComponent<RectTransform>();

            GameObject goCreate = GameObject.Instantiate(goImage) as GameObject;
            goCreate.name = "Image";
            goCreate.transform.SetParent(GameObject.Find("Canvas/Create").transform);
            rectCreate = goCreate.GetComponent<RectTransform>();
            rectCreate.anchoredPosition3D = rectImage.anchoredPosition3D;
            rectCreate.localScale = Vector3.one;

            //rectOldChild = goOld.transform.FindChild("child").GetComponent<RectTransform>();

            Doit();
        }

        private static void Doit()
        {
            Vector2 pivot = Vector2.one;
            Vector2 anchorsMin = new Vector2(0, 0);
            Vector2 anchorsMax = new Vector2(1, 1);

            if(XAxisIsPoint(anchorsMin, anchorsMax))
            {
                Debug.LogError("XIsPoint");
                SyncXAxis(rectCreate, pivot.x, anchorsMin.x, anchorsMax.x);
            }
            else
            {
                Debug.LogError("XNotPoint");
                SyncWidth(rectImage, rectCreate, pivot.x, anchorsMin.x, anchorsMax.x);
            }

            if(YAxisIsPoint(anchorsMin, anchorsMax))
            {
                Debug.LogError("YIsPoint");
                SyncYAxis(rectCreate, pivot.y, anchorsMin.y, anchorsMax.y);
            }
            else
            {
                Debug.LogError("YNotPoint");
                SyncHeight(rectImage, rectCreate, pivot.y, anchorsMin.y, anchorsMax.y);
            }

            Debug.LogError("offsetMin:" + rectCreate.offsetMin);
            Debug.LogError("offsetMax:" + rectCreate.offsetMax);
        }

        private static void SyncXAxis(RectTransform rect, float pivotX, float minX, float maxX)
        {
            Vector2 temp = rect.position;
            //改变锚点 保持坐标不变
            SetAnchorMinX(rect, minX);
            SetAnchorMaxX(rect, maxX);
            rect.position = temp;

            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            //改变中心轴位置
            float deltaX = (pivotX - pivot.x) * rect.sizeDelta.x;
            SetPivotX(rect, pivotX);
            SetAnchorPositionX(rect, anchoredPosition.x + deltaX);
        }

        private static void SyncYAxis(RectTransform rect, float pivotY, float minY, float maxY)
        {
            Vector2 temp = rect.position;
            //改变锚点 保持坐标不变
            SetAnchorMinY(rect, minY);
            SetAnchorMaxY(rect, maxY);
            rect.position = temp;

            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            //改变中心轴位置
            float deltaY = (pivotY - pivot.y) * rect.sizeDelta.y;
            SetPivotY(rect, pivotY);
            SetAnchorPositionY(rect, anchoredPosition.y + deltaY);
        }


        private static bool XAxisIsPoint(Vector2 anchorsMin, Vector2 anchorsMax)
        {
            return anchorsMin.x == anchorsMax.x;
        }

        private static bool YAxisIsPoint(Vector2 anchorsMin, Vector2 anchorsMax)
        {
            return anchorsMin.y == anchorsMax.y;
        }

        private static void SyncWidth(RectTransform rect, RectTransform rectCreate, float pivotX, float minX, float maxX)
        {
            Vector2 size = rect.sizeDelta;
            Vector2 pivot = rect.pivot;
            Vector2 anchoredPostion = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetX = 0;
            float maxOffsetX = 0;
            if(XAxisIsPoint(rect.anchorMin, rect.anchorMax))
            {
                //转换pivot x为0时 anchoredPosition
                float childLeft = anchoredPostion.x - pivot.x * size.x;
                float anchorX = rect.anchorMin.x;
                //转换anchored x为0时 anchoredPosition
                minOffsetX = childLeft + anchorX * parentRect.rect.width;
                maxOffsetX = parentRect.rect.width - (minOffsetX + size.x);
            }
            else
            {
                throw new Exception("error 暂时不存在这种解析");
            }
            
            //改变锚点
            SetAnchorMinX(rectCreate, minX);
            SetAnchorMaxX(rectCreate, maxX);

            SetOffsetMinX(rectCreate, minOffsetX);
            SetOffsetMaxX(rectCreate, -maxOffsetX);
        }

        private static void SyncHeight(RectTransform rect, RectTransform rectCreate, float pivotY, float minY, float maxY)
        {
            Vector2 size = rect.sizeDelta;
            Vector2 pivot = rect.pivot;
            Vector2 anchoredPostion = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetY = 0;
            float maxOffsetY = 0;
            if(YAxisIsPoint(rect.anchorMin, rect.anchorMax))
            {
                float height = parentRect.rect.height;
                float childBottom = anchoredPostion.y - pivot.y * size.y;
                float anchorY = rect.anchorMin.y;
                minOffsetY = childBottom + anchorY * height;
                maxOffsetY = height - (minOffsetY + size.y);
            }
            else
            {
                throw new Exception("error 暂时不存在这种解析");
            }

            Debug.LogError("minOffsetY:" + minOffsetY);
            Debug.LogError("maxOffsetY:" + maxOffsetY);

            //改变锚点
            SetAnchorMinY(rectCreate, minY);
            SetAnchorMaxY(rectCreate, maxY);

            SetOffsetMinY(rectCreate, minOffsetY);
            SetOffsetMaxY(rectCreate, -maxOffsetY);
        }

        private static void SetAnchorMinX(RectTransform rect, float x)
        {
            float y = rect.anchorMin.y;
            rect.anchorMin = new Vector2(x, y);
        }

        private static void SetAnchorMinY(RectTransform rect, float y)
        {
            float x = rect.anchorMin.x;
            rect.anchorMin = new Vector2(x, y);
        }

        private static void SetAnchorMaxX(RectTransform rect, float x)
        {
            float y = rect.anchorMax.y;
            rect.anchorMax = new Vector2(x, y);
        }

        private static void SetAnchorMaxY(RectTransform rect, float y)
        {
            float x = rect.anchorMax.x;
            rect.anchorMax = new Vector2(x, y);
        }

        private static void SetPivotX(RectTransform rect, float x)
        {
            float y = rect.pivot.y;
            rect.pivot = new Vector2(x, y);
        }

        private static void SetPivotY(RectTransform rect, float y)
        {
            float x = rect.pivot.x;
            rect.pivot = new Vector2(x, y);
        }

        private static void SetAnchorPositionX(RectTransform rect, float x)
        {
            float y = rect.anchoredPosition.y;
            rect.anchoredPosition3D = new Vector3(x, y, 0);
        }

        private static void SetAnchorPositionY(RectTransform rect, float y)
        {
            float x = rect.anchoredPosition.x;
            rect.anchoredPosition3D = new Vector3(x, y, 0);
        }

        private static void SetOffsetMinX(RectTransform rect, float x)
        {
            float y = rect.offsetMin.y;
            rect.offsetMin = new Vector2(x, y);
        }

        private static void SetOffsetMinY(RectTransform rect, float y)
        {
            float x = rect.offsetMin.x;
            rect.offsetMin = new Vector2(x, y);
        }

        private static void SetOffsetMaxX(RectTransform rect, float x)
        {
            float y = rect.offsetMax.y;
            rect.offsetMax = new Vector2(x, y);
        }

        private static void SetOffsetMaxY(RectTransform rect, float y)
        {
            float x = rect.offsetMax.x;
            rect.offsetMax = new Vector2(x, y);
        }
    }
}
