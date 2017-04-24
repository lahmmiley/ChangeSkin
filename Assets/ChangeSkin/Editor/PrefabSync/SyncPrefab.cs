using System.Collections.Generic;
using System.Linq;
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
            Vector3 tempPosition = rectCreate.position;
            rectCreate.anchorMin = rectOld.anchorMin;
            rectCreate.anchorMax = rectOld.anchorMax;
            rectCreate.pivot = rectOld.pivot;
            rectCreate.position = tempPosition;

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

    }
}
