using Psd2UGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PrefabSync
{
    public class PrefabComparison
    {
        //private static Dictionary<string, Type> checkTypeDict = new Dictionary<string, Type>()
        //{
        //    {NodeType.TEXT, typeof(Text)},
        //    {NodeType.IMAGE, typeof(Image)},
        //    {NodeType.MASK, typeof(Mask)},
        //    {NodeType.BUTTON, typeof(Button)},
        //    {NodeType.CUSTOM_BUTTON, typeof(CustomButton)},
        //    {NodeType.ENTER_EXIT_BUTTON, typeof(CustomEnterExsitButton)},
        //    {NodeType.SCROLL_RECT, typeof(ScrollRect)},
        //    {NodeType.TOGGLE, typeof(Toggle)},
        //    {NodeType.TOGGLE_GROUP, typeof(ToggleGroup)},
        //    {NodeType.INPUT, typeof(Input)},
        //    {NodeType.CANVAS, typeof(Canvas)},
        //};

        private static HashSet<string> _addPathHash;
        private static HashSet<string> _inexistPathHash;

        public static void Compare(GameObject goOld, GameObject goNew)
        {
            string path = string.Empty;
            _addPathHash = new HashSet<string>();
            _inexistPathHash = new HashSet<string>();
            _traverseGameObject(path, goOld, goNew);
            //Printer.Print(_addPathHash, "新添加的路径\n");
            //Printer.Print(_inexistPathHash, "缺失的路径\n");
        }

        private static void _traverseGameObject(string currentPath, GameObject goOld, GameObject goNew)
        {
            RectTransform rectOld = goOld.GetComponent<RectTransform>();
            RectTransform rectNew = goNew.GetComponent<RectTransform>();

            CompareComponent(currentPath, goOld, goNew);

            Dictionary<string, int> oldNameDict = GetNameDict(rectOld);
            string[] oldNameArray = oldNameDict.Keys.ToArray<string>();

            Dictionary<string, int> newNameDict = GetNameDict(rectNew);
            string[] newNameArray = newNameDict.Keys.ToArray<string>();

            RecordAddPath(currentPath, oldNameArray, newNameArray);
            RecordInexistPath(currentPath, oldNameArray, newNameArray);

            IEnumerable<string> intersectEnum = oldNameArray.Intersect<string>(newNameArray);
            if(intersectEnum  != null)
            {
                foreach(string name in intersectEnum)
                {
                    GameObject goChildOld = rectOld.Find(name).gameObject;
                    GameObject goChildNew = rectNew.Find(name).gameObject;
                    string childPath = currentPath + name + "/";
                    _traverseGameObject(childPath, goChildOld, goChildNew);
                }
            }
        }

        private static void CompareComponent(string currentPath, GameObject goOld, GameObject goNew)
        {
        }

        private static void RecordAddPath(string currentPath, string[] oldNameArray, string[] newNameArray)
        {
            IEnumerable<string> addEnum = newNameArray.Except<string>(oldNameArray);
            if(addEnum != null)
            {
                foreach(string name in addEnum)
                {
                    _addPathHash.Add(currentPath + name + "/");
                }
            }
        }

        private static void RecordInexistPath(string currentPath, string[] oldNameArray, string[] createNameArray)
        {
            IEnumerable<string> inexistEnum = oldNameArray.Except<string>(createNameArray);
            if(inexistEnum != null)
            {
                foreach(string name in inexistEnum)
                {
                    _inexistPathHash.Add(currentPath + name + "/");
                }
            }
        }

        private static Dictionary<string, int> GetNameDict(RectTransform rect)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for(int i = 0; i < rect.childCount; i++)
            {
                string name = rect.GetChild(i).name;
                if(!dict.ContainsKey(name))
                {
                    dict.Add(name, 0);
                }
            }
            return dict;
        }
    }
}
