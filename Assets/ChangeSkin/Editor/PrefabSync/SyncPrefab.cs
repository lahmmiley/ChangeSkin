using System;
using System.Collections.Generic;
using System.Linq;
using Tool;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
            Printer.Print(_inexistPathHash, "缺失的路径\n");
        }

        private static void _traversalGameObject(string path, GameObject goOld, GameObject goCreate)
        {
            RectTransform rectOld = goOld.GetComponent<RectTransform>();
            RectTransform rectCreate = goCreate.GetComponent<RectTransform>();
            SyncAnchorAndPivot(rectCreate, rectOld.pivot, rectOld.anchorMin, rectOld.anchorMax);
            ReplaceImage(goOld, goCreate);

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
                    GameObject goChildOld = goOld.transform.Find(name).gameObject;
                    GameObject goInexist = GameObject.Instantiate(goChildOld) as GameObject;
                    goInexist.transform.SetParent(rectCreate);
                    SyncPosition(goInexist, goChildOld);
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

            for(int i = 0; i < rectOld.childCount; i++)
            {
                string name = rectOld.GetChild(i).name;
                Transform child = rectCreate.Find(name);
                child.SetSiblingIndex(i);
            }
        }

        private static HashSet<string> _replacedHash = new HashSet<string>();
        private static void ReplaceImage(GameObject goOld, GameObject goCreate)
        {
            Image imageOld = goOld.GetComponent<Image>();
            Image imageCreate = goCreate.GetComponent<Image>();
            if(imageOld == null)
            {
                return;
            }
            if((imageOld != null) && (imageCreate == null))
            {
                Debug.LogWarning("存在老预设有图片 而新预设没图片的情况");
                return;
            }

            //可能会出现sprite为空的情况
            if(imageOld.sprite == null)
            {
                return;
            }

            string path = AssetDatabase.GetAssetPath(imageOld.sprite);
            if(_replacedHash.Contains(path))
            {
                return;
            }
            _replacedHash.Add(path);
            Debug.LogError("替换图片:" + path);
            string createPath = AssetDatabase.GetAssetPath(imageCreate.sprite);

            FileUtil.DeleteFileOrDirectory(path);
            FileUtil.CopyFileOrDirectory(createPath, path);
            imageCreate.sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
            AssetDatabase.Refresh();
        }

        private static void SyncPosition(GameObject goCreate, GameObject goOld)
        {
            RectTransform rectCreate = goCreate.GetComponent<RectTransform>();
            RectTransform rectOld = goOld.GetComponent<RectTransform>();
            rectCreate.name = rectOld.name;
            rectCreate.pivot = rectOld.pivot;
            rectCreate.anchorMin = rectOld.anchorMin;
            rectCreate.anchorMax = rectOld.anchorMax;
            rectCreate.anchoredPosition3D = rectOld.anchoredPosition3D;
            rectCreate.localRotation = rectOld.localRotation;
            rectCreate.localScale = rectOld.localScale;
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

        private static void SyncAnchorAndPivot(RectTransform rect, Vector2 pivot, Vector2 anchorsMin, Vector2 anchorsMax)
        {
            //TODO
            if(XAxisIsPoint(anchorsMin, anchorsMax))
            {
                //Debug.LogError("XIsPoint");
                SyncXAxis(rect, pivot.x, anchorsMin.x, anchorsMax.x);
            }
            else
            {
                //Debug.LogError("XNotPoint");
                SyncWidth(rect, pivot.x, anchorsMin.x, anchorsMax.x);
            }

            if(YAxisIsPoint(anchorsMin, anchorsMax))
            {
                //Debug.LogError("YIsPoint");
                SyncYAxis(rect, pivot.y, anchorsMin.y, anchorsMax.y);
            }
            else
            {
                //Debug.LogError("YNotPoint");
                SyncHeight(rect, pivot.y, anchorsMin.y, anchorsMax.y);
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

        private static void SyncWidth(RectTransform rect, float pivotX, float minX, float maxX)
        {
            Vector2 size = rect.sizeDelta;
            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetX = 0;
            float maxOffsetX = 0;
            if(XAxisIsPoint(rect.anchorMin, rect.anchorMax))
            {
                //转换pivot x为0时 anchoredPosition
                float left = anchoredPosition.x - pivot.x * size.x;
                float anchorX = rect.anchorMin.x;
                //转换anchored x为0时 anchoredPosition
                left = left + anchorX * parentRect.rect.width;
                minOffsetX = left - minX * parentRect.rect.width;
                maxOffsetX = (left + size.x) - maxX * parentRect.rect.width;
            }
            else
            {
                throw new Exception("error 暂时不存在这种解析");
            }
            
            //改变锚点
            TransformUtility.SetAnchorMinX(rect, minX);
            TransformUtility.SetAnchorMaxX(rect, maxX);
            TransformUtility.SetPivotX(rect, pivotX);

            TransformUtility.SetOffsetMinX(rect, minOffsetX);
            TransformUtility.SetOffsetMaxX(rect, maxOffsetX);
        }

        private static void SyncHeight(RectTransform rect, float pivotY, float minY, float maxY)
        {
            Vector2 size = rect.sizeDelta;
            Vector2 pivot = rect.pivot;
            Vector2 anchoredPosition = rect.anchoredPosition;
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();

            float minOffsetY = 0;
            float maxOffsetY = 0;
            if (YAxisIsPoint(rect.anchorMin, rect.anchorMax))
            {
                float height = parentRect.rect.height;
                float bottom = anchoredPosition.y - pivot.y * size.y;
                float anchorY = rect.anchorMin.y;
                bottom = bottom + anchorY * height;
                minOffsetY = bottom - minY * height;
                maxOffsetY = (bottom + size.y) - height * maxY;
            }
            else
            {
                throw new Exception("error 暂时不存在这种解析");
            }

            //改变锚点
            TransformUtility.SetAnchorMinY(rect, minY);
            TransformUtility.SetAnchorMaxY(rect, maxY);
            TransformUtility.SetPivotY(rect, pivotY);

            TransformUtility.SetOffsetMinY(rect, minOffsetY);
            TransformUtility.SetOffsetMaxY(rect, maxOffsetY);
        }

    }
}
