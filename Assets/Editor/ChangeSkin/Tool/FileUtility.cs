using LitJson;
using Psd2UGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public class FileUtility
    {
        public const string PSD_IMAGE_DIR = "Assets/PsdResources/Image/";
        public static string UI_DATA_DIR = PSD_IMAGE_DIR.Replace("Image", "Data");
        public const string TEXTURE_DIR = "Assets/Textures/";
        public const string TEXTURE_MODULE_ATLAS_DIR = "Assets/Textures/UI/";
        public const string MAP_DIR = "Assets/Textures/Maps/";

        public const string RESOURCE_SPRITE_DIR = "Assets/Resources/Sprite/";

        public const string JSON_POSTFIX = ".json";
        public const string PNG_POSTFIX = ".png";
        public const string PREFAB_POSTFIX = ".prefab";
        public const string XIUXIAN_POSTFIX = "XX";

        public static string AllPath2AssetPath(string allPath)
        {
            return allPath.Substring(allPath.IndexOf("Assets"));
        }
        
        public static string GetJsonPath(string fileName)
        {
            return UI_DATA_DIR + fileName + JSON_POSTFIX;
        }

        public static string GetFileName(string path)
        {
            int startIndex = path.LastIndexOf('/') + 1;
            int endIndex = path.LastIndexOf('.');
            return path.Substring(startIndex, endIndex - startIndex);
        }

        public static string GetFileNameWithPostfix(string path)
        {
            int startIndex = path.LastIndexOf('/') + 1;
            return path.Substring(startIndex, path.Length - startIndex);
        }

        public static string GetFolderName(string path)
        {
            int endIndex = path.LastIndexOf('/');
            int startIndex = path.LastIndexOf('/', endIndex - 1) + 1;
            return path.Substring(startIndex, endIndex - startIndex);
        }

        public static string RemovePostfix(string path)
        {
            if(path.IndexOf('.') != -1)
            {
                return path.Substring(0, path.IndexOf('.'));
            }
            return path;
        }

        public static void RecreateDirectory(string dir)
        {
            RemoveDirectory(dir);
            Directory.CreateDirectory(dir);
        }

        public static void CreateDirectory(string dir)
        {
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void RemoveDirectory(string dir)
        {
            if(Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
        }

        public static Dictionary<string, string[]> GetSliceDict(string jsonPath)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            StreamReader sr = new StreamReader(jsonPath);
            string content = sr.ReadToEnd();
            sr.Close();
            JsonData jsonData = JsonMapper.ToObject(content);
            _getSilceDict(jsonData, dict);
            return dict;
        }

        private static void _getSilceDict(JsonData jsonData, Dictionary<string, string[]> dict)
        {
            string typeStr = jsonData[NodeField.TYPE].ToString().ToLower();
            if ((typeStr == NodeType.IMAGE) && (jsonData.Keys.Contains(NodeField.SLICE)))
            {
                string name = jsonData[NodeField.NAME].ToString();
                if (!dict.ContainsKey(name))
                {
                    string param = jsonData[NodeField.SLICE].ToString();
                    string[] splitArray = param.Split(',');
                    dict.Add(name, splitArray);
                }
            }
            if (jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                int length = jsonData[NodeField.CHILDREN].Count;
                JsonData children = jsonData[NodeField.CHILDREN];
                for (int i = 0; i < length; i++)
                {
                    _getSilceDict(children[i], dict);
                }
            }

        }
    }
}