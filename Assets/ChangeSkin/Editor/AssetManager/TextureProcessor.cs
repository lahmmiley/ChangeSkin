using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tool;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class NodeField
    {
        public const string NAME = "Name";
        public const string TYPE = "Type";
        public const string X = "X";
        public const string Y = "Y";
        public const string WIDTH = "Width";
        public const string HEIGHT = "Height";
        public const string CHILDREN = "Children";
        public const string PARAM = "Param";
        public const string BELONG_PSD = "BelongPsd";
        public const string SIZE = "Size";
        public const string TEXT = "Text";
    }

    public class TextureProcessor : AssetPostprocessor
    {
        private static Dictionary<string, Dictionary<string, Vector4>> _jsonDict = new Dictionary<string, Dictionary<string, Vector4>>();

        void OnPreprocessSprites()
        {
            LogError("Sprites:" + assetPath);
        }

        void OnPreprocessTexture()
        {
            if (assetPath.StartsWith(FileUtility.UI_IMAGE_DIR))
            {
                FormatUITexture(assetPath);
            }
        }

        private void FormatUITexture(string assetPath)
        {
            string folderName = FileUtility.GetFolderName(assetPath);
            ReadSilce(folderName);
            string fileName = FileUtility.GetFileName(assetPath);
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = false;
            textureImporter.spritePackingTag = folderName;
            Dictionary<string, Vector4> sliceDict = _jsonDict[folderName];
            if(sliceDict.Keys.Contains(fileName))
            {
                textureImporter.spriteBorder = sliceDict[fileName];
            }
        }

        private void ReadSilce(string folderName)
        {
            if(_jsonDict.ContainsKey(folderName))
            {
                return;
            }
            Dictionary<string, Vector4> sliceDict = new Dictionary<string, Vector4>();
            _jsonDict.Add(folderName, sliceDict);
            string jsonPath = FileUtility.GetJsonPath(folderName);
            StreamReader sr = new StreamReader(jsonPath);
            string content = sr.ReadToEnd();
            JsonData jsonData = JsonMapper.ToObject(content);
            TraversalTree(jsonData, sliceDict);
        }

        private static void TraversalTree(JsonData jsonData, Dictionary<string, Vector4> sliceDict)
        {
            string typeStr = jsonData[NodeField.TYPE].ToString().ToLower();
            if ((typeStr == "image") && (jsonData.Keys.Contains(NodeField.PARAM)))
            {
                string name = jsonData[NodeField.NAME].ToString();
                if(!sliceDict.ContainsKey(name))
                {
                    string param = jsonData[NodeField.PARAM].ToString();
                    string[] splitArray = param.Split(',');
                    Vector4 v4 = new Vector4(float.Parse(splitArray[2]), float.Parse(splitArray[1]), float.Parse(splitArray[3]), float.Parse(splitArray[0]));//0:上 1:下 2:左 3:右
                    sliceDict.Add(name, v4);

                }
            }
            if (jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                int length = jsonData[NodeField.CHILDREN].Count;
                JsonData children = jsonData[NodeField.CHILDREN];
                for (int i = 0; i < length; i++)
                {
                    TraversalTree(children[i], sliceDict);
                }
            }
        }
    }
}
