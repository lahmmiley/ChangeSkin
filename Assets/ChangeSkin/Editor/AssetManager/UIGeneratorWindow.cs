using Psd2UGUI;
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
            Rect rect = new Rect(800, 0, 400, 200);
            UIGeneratorWindow window = (UIGeneratorWindow)EditorWindow.GetWindowWithRect<UIGeneratorWindow>(rect, true, "yes");
            window.Show();
        }

        private string _errorMessage;
        private Object _jsonFile;

        private void OnGUI()
        {
            _jsonFile = EditorGUILayout.ObjectField(_jsonFile, typeof(TextAsset), false, null);
            _errorMessage = "请选择Json文件";
            if(_jsonFile != null)
            {
                string path = AssetDatabase.GetAssetPath(_jsonFile);
                if(path.EndsWith(".json"))
                {
                    if(GUILayout.Button("生成资源", GUILayout.Width(100)))
                    {
                        DevelopResourcesGenerator.Generate(path);
                        _errorMessage = "生成资源成功";
                    }
                    if(GUILayout.Button("生成prefab", GUILayout.Width(100)))
                    {
                        PrefabCreator.Instance.Create(FileUtility.GetFileName(path));
                        _errorMessage = "生成prefab成功";
                    }
                }
                else
                {
                    _jsonFile = null;
                    _errorMessage = "<color=#FF0000>选择的文件格式并非Json文件</color>";
                }
            }

            EditorGUILayout.LabelField(_errorMessage);
        }
    }
}
