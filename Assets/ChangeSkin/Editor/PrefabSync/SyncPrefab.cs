using System;
using System.Collections.Generic;
using System.Linq;
using Tool;
using UnityEngine;

namespace PrefabSync
{
    public class SyncPrefab
    {
        private static HashSet<string> _addPathHash = new HashSet<string>();
        private static HashSet<string> _inexistPathHash = new HashSet<string>();

        public static void ComparePrefab(GameObject goOld, GameObject goCreate)
        {
            string path = string.Empty;
            _traversalGameObject(path, goOld, goCreate);
            //Printer.Print(_addPathHash, "新添加的路径\n");
            //Printer.Print(_inexistPathHash, "缺失的路径\n");
        }

        private static void _traversalGameObject(string path, GameObject goOld, GameObject goCreate)
        {
            RectTransform rectOld = goOld.GetComponent<RectTransform>();
            RectTransform rectCreate = goCreate.GetComponent<RectTransform>();
            Doit(rectOld, rectCreate, rectOld.pivot, rectOld.anchorMin, rectOld.anchorMax);

            Dictionary<string, int> oldNameDict = GetNameDict(rectOld);
            string[] oldNameArray = oldNameDict.Keys.ToArray<string>();

            Dictionary<string, int> createNameDict = GetNameDict(rectCreate);
            string[] createNameArray = createNameDict.Keys.ToArray<string>();


            IEnumerable<string> addEnum = createNameArray.Except<string>(oldNameArray);
            if(addEnum != null)
            {
                foreach(string name in addEnum)
                {
                    _addPathHash.Add(path + name + "/");
                }
            }

            IEnumerable<string> inexistEnum = oldNameArray.Except<string>(createNameArray);
            if(inexistEnum != null)
            {
                foreach(string name in inexistEnum)
                {
                    _inexistPathHash.Add(path + name + "/");
                }
            }

            IEnumerable<string> intersectEnum = oldNameArray.Intersect<string>(createNameArray);
            if(intersectEnum  != null)
            {
                //TODO 顺序判断 不对则警告
                foreach(string name in intersectEnum)
                {
                    GameObject goChildOld = rectOld.Find(name).gameObject;
                    GameObject goChildCreate = rectCreate.Find(name).gameObject;
                    string currentPath = path + name + "/";
                    _traversalGameObject(currentPath, goChildOld, goChildCreate);
                }
            }
        }

        private static Dictionary<string, int> GetNameDict(RectTransform rect)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for(int i = 0; i < rect.childCount; i++)
            {
                dict.Add(rect.GetChild(i).name, i);
            }
            return dict;
        }

        private static void Doit(RectTransform rectOld, RectTransform rectCreate, Vector2 pivot, Vector2 anchorsMin, Vector2 anchorsMax)
        {
            if(XAxisIsPoint(anchorsMin, anchorsMax))
            {
                //Debug.LogError("XIsPoint");
                SyncXAxis(rectCreate, pivot.x, anchorsMin.x, anchorsMax.x);
            }
            else
            {
                //Debug.LogError("XNotPoint");
                SyncWidth(rectOld, rectCreate, pivot.x, anchorsMin.x, anchorsMax.x);
            }

            if(YAxisIsPoint(anchorsMin, anchorsMax))
            {
                //Debug.LogError("YIsPoint");
                SyncYAxis(rectCreate, pivot.y, anchorsMin.y, anchorsMax.y);
            }
            else
            {
                //Debug.LogError("YNotPoint");
                SyncHeight(rectOld, rectCreate, pivot.y, anchorsMin.y, anchorsMax.y);
            }
        }

        private static void SyncXAxis(RectTransform rect, float pivotX, float minX, float maxX)
        {
            Vector2 temp = rect.position;
            //改变锚点 保持坐标不变
            TransformUtility.SetAnchorMinX(rect, minX);
            TransformUtility.SetAnchorMaxX(rect, maxX);
            rect.position = temp;

            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            //改变中心轴位置
            float deltaX = (pivotX - pivot.x) * rect.sizeDelta.x;
            TransformUtility.SetPivotX(rect, pivotX);
            TransformUtility.SetAnchorPositionX(rect, anchoredPosition.x + deltaX);
        }

        private static void SyncYAxis(RectTransform rect, float pivotY, float minY, float maxY)
        {
            Vector2 temp = rect.position;
            //改变锚点 保持坐标不变
            TransformUtility.SetAnchorMinY(rect, minY);
            TransformUtility.SetAnchorMaxY(rect, maxY);
            rect.position = temp;

            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            //改变中心轴位置
            float deltaY = (pivotY - pivot.y) * rect.sizeDelta.y;
            TransformUtility.SetPivotY(rect, pivotY);
            TransformUtility.SetAnchorPositionY(rect, anchoredPosition.y + deltaY);
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
            Vector2 anchoredPosition = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetX = 0;
            float maxOffsetX = 0;
            //XAxis一定不是SamePoint
            //if(XAxisIsPoint(rect.anchorMin, rect.anchorMax))
            //{
            //    //XAxis一定不是SamePoint
            //    Debug.LogError("nimei");
            //    //转换pivot x为0时 anchoredPosition
            //    float childLeft = anchoredPosition.x - pivot.x * size.x;
            //    float anchorX = rect.anchorMin.x;
            //    //转换anchored x为0时 anchoredPosition
            //    minOffsetX = childLeft + anchorX * parentRect.rect.width;
            //    maxOffsetX = parentRect.rect.width - (minOffsetX + size.x);
            //}
            //else
            {
                //转换pivot x为0时 anchoredPosition
                float childLeft = anchoredPosition.x - pivot.x * size.x;
                float anchorX = (rect.anchorMin.x + rect.anchorMax.x) * 0.5f;
                //转换anchored x为0时 anchoredPosition
                minOffsetX = childLeft + anchorX * parentRect.rect.width;
                maxOffsetX = parentRect.rect.width - (minOffsetX + rect.rect.width);
            }
            
            //改变锚点
            TransformUtility.SetAnchorMinX(rectCreate, minX);
            TransformUtility.SetAnchorMaxX(rectCreate, maxX);

            TransformUtility.SetOffsetMinX(rectCreate, minOffsetX);
            TransformUtility.SetOffsetMaxX(rectCreate, -maxOffsetX);
        }

        private static void SyncHeight(RectTransform rect, RectTransform rectCreate, float pivotY, float minY, float maxY)
        {
            Vector2 size = rect.sizeDelta;
            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetY = 0;
            float maxOffsetY = 0;
            float height = parentRect.rect.height;
            if(YAxisIsPoint(rect.anchorMin, rect.anchorMax))
            {
                float childBottom = anchoredPosition.y - pivot.y * size.y;
                float anchorY = rect.anchorMin.y;
                minOffsetY = childBottom + anchorY * height;
                maxOffsetY = height - (minOffsetY + size.y);
            }
            else
            {
                float childBottom = anchoredPosition.y - pivot.y * size.y;
                float anchorY = rect.anchorMin.y;
                minOffsetY = childBottom + anchorY * height;
                maxOffsetY = height - (minOffsetY + rect.rect.height);
            }

            //改变锚点
            TransformUtility.SetAnchorMinY(rectCreate, minY);
            TransformUtility.SetAnchorMaxY(rectCreate, maxY);

            TransformUtility.SetOffsetMinY(rectCreate, minOffsetY);
            TransformUtility.SetOffsetMaxY(rectCreate, -maxOffsetY);
        }

    }
}
