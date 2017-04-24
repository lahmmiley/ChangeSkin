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
            SyncPrefabWindow window = (SyncPrefabWindow)EditorWindow.GetWindowWithRect<SyncPrefabWindow>(rect, true, "同步预设");
            window.Show();
        }

        private Object _newPrefab;
        private Object _oldPrefab;
        private Object _createPrefab;

        private void OnGUI()
        {
            _oldPrefab = EditorGUILayout.ObjectField("oldPrefab:", _oldPrefab, typeof(Object), true);
            _newPrefab = EditorGUILayout.ObjectField("newPrefab:", _newPrefab, typeof(Object), true);
            _createPrefab = EditorGUILayout.ObjectField("createPrefab:", _createPrefab, typeof(Object), true);

            if((_newPrefab != null) && (_oldPrefab != null))
            {
                if(GUILayout.Button("对比生成", GUILayout.Width(100)))
                {
                    if(_createPrefab == null)
                    {
                        GameObject goCreate = GameObject.Instantiate(_newPrefab) as GameObject;
                        goCreate.name = _newPrefab.name;
                        goCreate.transform.SetParent(GameObject.Find("Canvas/Create").transform);
                        RectTransform rectCreate = goCreate.GetComponent<RectTransform>();
                        RectTransform rectNew = (_newPrefab as GameObject).GetComponent<RectTransform>();
                        goCreate.SetActive(true);
                        rectCreate.anchorMin = rectNew.anchorMin;
                        rectCreate.anchorMax = rectNew.anchorMax;
                        rectCreate.pivot = rectNew.pivot;
                        rectCreate.localRotation = rectNew.localRotation;
                        rectCreate.localScale = rectNew.localScale;
                        rectCreate.anchoredPosition3D = rectNew.anchoredPosition3D;
                        _createPrefab = goCreate;
                    }
                    SyncPrefab.ComparePrefab(_oldPrefab as GameObject, _createPrefab as GameObject);
                }
            }
        }
    }
}