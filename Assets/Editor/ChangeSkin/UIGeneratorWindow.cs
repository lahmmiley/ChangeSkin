using PrefabSync;
using Psd2UGUI;
using System.IO;
using Tool;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace AssetManager
{
    public class UIGeneratorWindow : EditorWindow
    {
        class JsonFileData
        {
            public string PopupContent;
            public string Path;
            public int Index;

            public JsonFileData(string guid, int index)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                this.Path = path;
                string fileName = FileUtility.GetFileName(path);
                this.PopupContent = GeneratorPopupContent(fileName);
                this.Index = index;
            }

            public string GeneratorPopupContent(string fileName)
            {
                string firstChar = fileName.Substring(0, 1);
                string firstPopup = firstChar.ToUpper() + "/";
                return firstPopup + fileName;
            }

            public static int Sort(JsonFileData data1, JsonFileData data2)
            {
                return data1.PopupContent.CompareTo(data2.PopupContent);
            }
        }

        [MenuItem("Psd2UGUI/Window")]
        static void Init()
        {
            Rect rect = new Rect(800, 0, 400, 300);
            UIGeneratorWindow window = (UIGeneratorWindow)EditorWindow.GetWindowWithRect<UIGeneratorWindow>(rect, true, "UIGenerator");
            window.Show();
        }

        private int _jsonFileIndex = -1;
        private List<JsonFileData> _jsonFileList;
        private string _errorMessage;

        private Object _jsonFile;
        private Object _newPrefab;
        private Object _oldPrefab;
        private Object _createPrefab;


        private void Awake()
        {
            InitJsonList();
        }

        private void OnProjectChange()
        {
            InitJsonList();
        }

        private void OnGUI()
        {
            _errorMessage = "";
            if(_jsonFileList == null)
            {
                InitJsonList();//预防在打开界面的时候调整代码，_jsonFileList数据会被清空
            }
            EditorGUILayout.LabelField("Psd2Prefab");
            OnGUIJsonFile();
            OnGUIButton();
            OnGUIErrorMessage();
        }

        private void OnGUIJsonFile()
        {
            EditorGUILayout.BeginHorizontal();
            _jsonFileIndex = EditorGUILayout.IntPopup("请Json文件:", _jsonFileIndex, GetDisplayOptions(), GetOptionValues(), new GUILayoutOption[]{ });
            if(_jsonFileIndex != -1)
            {
                _jsonFile = AssetDatabase.LoadAssetAtPath(_jsonFileList[_jsonFileIndex].Path, typeof(TextAsset)) as TextAsset;
                _jsonFileIndex = -1;
            }
            _jsonFile = EditorGUILayout.ObjectField(_jsonFile, typeof(TextAsset), false, null);
            EditorGUILayout.EndHorizontal();
        }

        private void OnGUIButton()
        {
            if(GUILayout.Button("生成资源", GUILayout.Width(100)))
            {
                if(_jsonFile == null) 
                {
                    _errorMessage = "<color=#ff0000>请先选择Json文件</color>";
                    return;
                }
                string path = AssetDatabase.GetAssetPath(_jsonFile);
                DevelopResourcesGenerator.Generate(path);
            }
            if(GUILayout.Button("生成prefab", GUILayout.Width(100)))
            {
                if(_jsonFile == null) 
                {
                    _errorMessage = "<color=#ff0000>请先选择Json文件</color>";
                    return;
                }
                string path = AssetDatabase.GetAssetPath(_jsonFile);
                PrefabCreator.Instance.Create(FileUtility.GetFileName(path));
            }
        }

        private void OnGUIErrorMessage()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.richText = true;
            guiStyle.fontSize = 20;
            guiStyle.wordWrap = true;
            if(_jsonFile != null)
            {
                string path = AssetDatabase.GetAssetPath(_jsonFile);
                if(!path.StartsWith(FileUtility.PSD_DATA_DIR))
                {
                    _errorMessage = "<color=#ff0000>请选择" + FileUtility.PSD_DATA_DIR + "目录下的文件!</color>";
                }
            }
            EditorGUILayout.LabelField(_errorMessage, guiStyle, null);
        }

        private void InitJsonList()
        {
            string[] guidList = AssetDatabase.FindAssets("", new string[] { FileUtility.PSD_DATA_DIR });
            _jsonFileList = new List<JsonFileData>();
            for (int i = 0; i < guidList.Length; i++)
            {
                JsonFileData data = new JsonFileData(guidList[i], i);
                _jsonFileList.Add(data);
            }
            _jsonFileList.Sort(JsonFileData.Sort);
        }

        private string[] GetDisplayOptions()
        {
            string[] result = new string[_jsonFileList.Count];
            for(int i = 0; i < _jsonFileList.Count; i++)
            {
                result[i] = _jsonFileList[i].PopupContent;
            }
            return result;
        }

        private int[] GetOptionValues()
        {
            int[] result = new int[_jsonFileList.Count];
            for(int i = 0; i < _jsonFileList.Count; i++)
            {
                result[i] = _jsonFileList[i].Index;
            }
            return result;
        }

        

    }
}
