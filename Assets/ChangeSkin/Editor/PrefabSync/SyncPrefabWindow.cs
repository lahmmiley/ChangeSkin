using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PrefabSync
{
    public class SyncPrefabWindow : EditorWindow
    {
        [MenuItem("Psd2UGUI/SyncPrefab")]
        static void Init()
        {
            Rect rect = new Rect(800, 0, 400, 200);
            SyncPrefabWindow window = (SyncPrefabWindow)EditorWindow.GetWindowWithRect<SyncPrefabWindow>(rect, true, "yes");
            window.Show();
        }

        private string _errorMessage;
        private string _prefabContent;
        private Object _newPrefab;
        private Object _oldPrefab;
        private Object _createPrefab;

        private HashSet<string> _inexistPathHash = new HashSet<string>();

        private void OnGUI()
        {
            _newPrefab = EditorGUILayout.ObjectField("newPrefab:", _newPrefab, typeof(Object), true, new GUILayoutOption[] { GUILayout.MinWidth(0) });
            _oldPrefab = EditorGUILayout.ObjectField("oldPrefab:", _oldPrefab, typeof(Object), true, new GUILayoutOption[] { GUILayout.MinWidth(100) });

            if((_newPrefab != null) && (_oldPrefab != null))
            {
                if(GUILayout.Button("对比生成", GUILayout.Width(100)))
                {
                    ComparePrefab(_newPrefab as GameObject, _oldPrefab as GameObject);
                }
            }
        }

        private void ComparePrefab(GameObject goNew, GameObject goOld)
        {
            string path = string.Empty;
            _traversalGameObject(path, goNew, goOld);
        }

        private Dictionary<string, int> nameDict = new Dictionary<string, int>();
        private void _traversalGameObject(string path, GameObject goNew, GameObject goOld)
        {
            RectTransform rectNew = goNew.GetComponent<RectTransform>();
            RectTransform rectOld = goNew.GetComponent<RectTransform>();
            rectNew.anchorMin = rectOld.anchorMin;
            rectNew.anchorMax = rectOld.anchorMax;
            rectNew.pivot = rectOld.pivot;

            //记录已有名字
            nameDict.Clear();
            for(int i = 0; i < rectOld.childCount; i++)
            {
                nameDict.Add(rectOld.GetChild(i).name, i);
            }
            string[] oldNameArray = nameDict.Keys.ToArray<string>();

            for(int i = 0; i < rectNew.childCount; i++)
            {
                GameObject goChildNew = rectNew.GetChild(i).gameObject;
                string name = goChildNew.name;
                string currentPath = path + name + "/";
                if(nameDict.ContainsKey(name))
                {
                    GameObject goChildOld = rectOld.GetChild(nameDict[name]).gameObject;
                    _traversalGameObject(currentPath, goChildNew, goChildOld);
                }
                else
                {
                    _inexistPathHash.Add(currentPath);
                }
            }

            //顺序乱了 警告
            if()

        }
    }
}