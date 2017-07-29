using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tool
{
    public static class TransformUtility
    {
        public static Vector2 HalfVector2 = new Vector2(0.5f, 0.5f);

        public static void SetAnchorMinX(RectTransform rect, float x)
        {
            float y = rect.anchorMin.y;
            rect.anchorMin = new Vector2(x, y);
        }

        public static void SetAnchorMinY(RectTransform rect, float y)
        {
            float x = rect.anchorMin.x;
            rect.anchorMin = new Vector2(x, y);
        }

        public static void SetAnchorMaxX(RectTransform rect, float x)
        {
            float y = rect.anchorMax.y;
            rect.anchorMax = new Vector2(x, y);
        }

        public static void SetAnchorMaxY(RectTransform rect, float y)
        {
            float x = rect.anchorMax.x;
            rect.anchorMax = new Vector2(x, y);
        }

        public static void SetPivotX(RectTransform rect, float x)
        {
            float y = rect.pivot.y;
            rect.pivot = new Vector2(x, y);
        }

        public static void SetPivotY(RectTransform rect, float y)
        {
            float x = rect.pivot.x;
            rect.pivot = new Vector2(x, y);
        }

        public static void SetAnchorPositionX(RectTransform rect, float x)
        {
            float y = rect.anchoredPosition.y;
            rect.anchoredPosition3D = new Vector3(x, y, 0);
        }

        public static void SetAnchorPositionY(RectTransform rect, float y)
        {
            float x = rect.anchoredPosition.x;
            rect.anchoredPosition3D = new Vector3(x, y, 0);
        }

        public static void SetOffsetMinX(RectTransform rect, float x)
        {
            float y = rect.offsetMin.y;
            rect.offsetMin = new Vector2(x, y);
        }

        public static void SetOffsetMinY(RectTransform rect, float y)
        {
            float x = rect.offsetMin.x;
            rect.offsetMin = new Vector2(x, y);
        }

        public static void SetOffsetMaxX(RectTransform rect, float x)
        {
            float y = rect.offsetMax.y;
            rect.offsetMax = new Vector2(x, y);
        }

        public static void SetOffsetMaxY(RectTransform rect, float y)
        {
            float x = rect.offsetMax.x;
            rect.offsetMax = new Vector2(x, y);
        }

        public static void MoveX(RectTransform rect, float x)
        {
            Vector3 position = rect.anchoredPosition3D;
            rect.anchoredPosition3D = new Vector3(position.x + x, position.y, position.z);
        }

        public static void MoveY(RectTransform rect, float y)
        {
            Vector3 position = rect.anchoredPosition3D;
            rect.anchoredPosition3D = new Vector3(position.x, position.y + y, position.z);
        }

        public static void SetPivot(RectTransform rect, Vector2 newPivot)
        {
            Vector2 sizeDelta = rect.sizeDelta;
            Vector2 oldPivot = rect.pivot;
            rect.pivot = newPivot;
            Vector3 position = rect.anchoredPosition;
            float newX = position.x + sizeDelta.x * (newPivot.x - oldPivot.x);
            float newY = position.y + sizeDelta.y * (newPivot.y - oldPivot.y);
            rect.anchoredPosition = new Vector2(newX, newY);
        }
    }
}