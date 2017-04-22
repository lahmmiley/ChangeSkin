using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Tool
{
    public class FileUtility
    {
        public const string UI_IMAGE_DIR = "Assets/UI/Image/";
        public static string UI_DATA_DIR = UI_IMAGE_DIR.Replace("Image", "Data");

        public const string RESOURCE_SPRITE_DIR = "Assets/Resources/Sprite/";

        public const string JSON_POSTFIX = ".json";
        public const string PNG_POSTFIX = ".png";
        public const string PREFAB_POSTFIX = ".prefab";

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

    }
}
