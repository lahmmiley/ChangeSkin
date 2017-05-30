using PrefabSync;
using Psd2UGUI;
using System.IO;
using Tool;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetManager
{
    public class UIGeneratorWindow : EditorWindow
    {
        [MenuItem("Psd2UGUI/Window")]
        static void Init()
        {
            Rect rect = new Rect(800, 0, 400, 300);
            UIGeneratorWindow window = (UIGeneratorWindow)EditorWindow.GetWindowWithRect<UIGeneratorWindow>(rect, true, "UIGenerator");
            window.Show();
        }

        private Object _jsonFile;
        private Object _newPrefab;
        private Object _oldPrefab;
        private Object _referencePrefab;
        private Object _createPrefab;

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Psd2Prefab");
            _jsonFile = EditorGUILayout.ObjectField(_jsonFile, typeof(TextAsset), false, null);
            if(_jsonFile != null)
            {
                string path = AssetDatabase.GetAssetPath(_jsonFile);
                if(path.EndsWith(".json"))
                {
                    if(GUILayout.Button("生成资源", GUILayout.Width(100)))
                    {
                        DevelopResourcesGenerator.Generate(path);
                    }
                    if(GUILayout.Button("生成prefab", GUILayout.Width(100)))
                    {
                        PrefabCreator.Instance.Create(FileUtility.GetFileName(path));
                    }
                }
                else
                {
                    _jsonFile = null;
                }
            }

            EditorGUILayout.LabelField("生成预设");
            _oldPrefab = EditorGUILayout.ObjectField("oldPrefab:", _oldPrefab, typeof(Object), true);
            _newPrefab = EditorGUILayout.ObjectField("newPrefab:", _newPrefab, typeof(Object), true);

            if((_newPrefab != null) && (_oldPrefab != null))
            {
                if(GUILayout.Button("生成预设", GUILayout.Width(100)))
                {
                    GameObject goCreate = CloneNewPrefab(_newPrefab);
                    SyncPrefab.ComparePrefab(_oldPrefab as GameObject, goCreate as GameObject);
                }
            }

            _referencePrefab = EditorGUILayout.ObjectField("referencePrefab:", _referencePrefab, typeof(Object), true);

            if(GUILayout.Button("保存参照预设", GUILayout.Width(100)))
            {
                SavePrefab(_referencePrefab);
            }

            //if(GUILayout.Button("生成参照预设", GUILayout.Width(100)))
            //{
            //    GameObject goCreate = CloneNewPrefab(_newPrefab);
            //    SyncPrefab.ComparePrefab(_referencePrefab as GameObject, goCreate as GameObject);
            //}
            //_createPrefab = EditorGUILayout.ObjectField("createPrefab:", _createPrefab, typeof(Object), true);
        }

        private void SavePrefab(Object prefab)
        {
            string dir = "Assets/Prefabs/UIReference/Backpack/";
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = dir + prefab.name + ".prefab";
            PrefabUtility.CreatePrefab(path, prefab as GameObject, ReplacePrefabOptions.ConnectToPrefab);
            AssetDatabase.Refresh();
        }

        private GameObject CloneNewPrefab(Object newPrefab)
        {
            GameObject goCreate = GameObject.Instantiate(newPrefab) as GameObject;
            goCreate.name = newPrefab.name;
            goCreate.transform.SetParent(GameObject.Find("Canvas/Create").transform);
            RectTransform rectCreate = goCreate.GetComponent<RectTransform>();
            RectTransform rectNew = (newPrefab as GameObject).GetComponent<RectTransform>();
            goCreate.SetActive(true);
            rectCreate.anchorMin = rectNew.anchorMin;
            rectCreate.anchorMax = rectNew.anchorMax;
            rectCreate.pivot = rectNew.pivot;
            rectCreate.localRotation = rectNew.localRotation;
            rectCreate.localScale = rectNew.localScale;
            rectCreate.anchoredPosition3D = rectNew.anchoredPosition3D;
            return goCreate;
        }
    }
}
