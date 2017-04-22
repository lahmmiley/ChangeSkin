using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Tool
{
    public class RuntimeResourcesGenerator
    {
        private static Dictionary<string, Vector4> _sliceDict = new Dictionary<string, Vector4>();

        public static void Generate(string jsonPath)
        {
            string jsonFileName = FileUtility.GetFileName(jsonPath);

            Read9SliceParam(jsonPath);

            Texture2D atlas = new Texture2D(2048, 2048);
            List<Texture2D> list = new List<Texture2D>();

            string imageDir = FileUtility.RemovePostfix(jsonPath.Replace("Data", "Image"));
            DirectoryInfo dirInfo = new DirectoryInfo(imageDir);
            List<string> nameList = new List<string>();
            foreach (FileInfo pngFile in dirInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly))
            {
                string pngAssetPath = FileUtility.AllPath2AssetPath(pngFile.FullName);
                CreateTextureImporter(pngAssetPath, true);
                AssetDatabase.ImportAsset(pngAssetPath, ImportAssetOptions.ForceUncompressedImport);
                //TODO
                //FormatTexture(FileTool.RemovePostfix(pngFile.Name), pngAssetPath);
                Texture2D texture = AssetDatabase.LoadAssetAtPath(pngAssetPath, typeof(Texture2D)) as Texture2D;
                nameList.Add(texture.name);
                list.Add(texture);
            }

            //padding REYES
            //如果图集太大，会有缩放 需要确认下是否会产生影响
            Rect[] rects = atlas.PackTextures(list.ToArray(), 5);
            atlas = AtlasOptimizer.OptimizeAtlas(atlas, rects);
            string outputPath = Application.dataPath + "/Atlas/" + jsonFileName + ".png";
            string assetPath = FileUtility.AllPath2AssetPath(outputPath);
            AssetDatabase.DeleteAsset(assetPath);
            SaveTexture(atlas, outputPath);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUncompressedImport);
            CreateTextureImporter(assetPath, true);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUncompressedImport);
            CreateMultipleModeSpriteImporter(assetPath, rects, atlas.width, atlas.height, nameList);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUncompressedImport);

            BuildAssetBundle(assetPath, jsonFileName);
        }

        private static void BuildAssetBundle(string assetPath, string jsonFileName)
        {
            //AssetBundleBuild assetBundle = new AssetBundleBuild();
            //assetBundle.assetNames = new string[] { assetPath };
            //assetBundle.assetBundleName = jsonFileName;

            ////依赖关系值得考虑
            ////TODO
            //BuildPipeline.BuildAssetBundles("Assets/AssetBundles", new AssetBundleBuild[1] { assetBundle }, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        }

        private static void SaveTexture(Texture2D texture, string outputPath)
        {
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes(outputPath, pngData);
        }

        private static void CreateTextureImporter(string texturePath, bool isReadable)
        {
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            importer.textureType = TextureImporterType.Advanced;
            importer.spriteImportMode = SpriteImportMode.None;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.filterMode = FilterMode.Bilinear;
            importer.maxTextureSize = 2048;
            importer.isReadable = isReadable;
            importer.mipmapEnabled = false;
            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            TextureImporterSettings setting = new TextureImporterSettings();
            importer.ReadTextureSettings(setting);
            importer.SetTextureSettings(setting);
        }

        private static void CreateMultipleModeSpriteImporter(string path, Rect[] rects, int width, int height, List<string> nameList)
        {
            //Debug.LogError(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            SpriteMetaData[] metaDatas = new SpriteMetaData[rects.Length];
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = nameList[i]; // texturesName[i].Replace(TEMP_TOKEN, "");
                Rect rect = rects[i];
                //Debug.Log(string.Format("{0} {1} {2} {3}", rect.xMin * width, rect.yMin * height,
                //    rect.width * width, rect.height));
                metaData.rect = new Rect(rect.xMin * width, rect.yMin * height,
                    rect.width * width, rect.height * height);
                metaData.pivot = new Vector2(0.5f, 0.5f);
                metaDatas[i] = metaData;
            }
            importer.spritesheet = metaDatas;
            importer.maxTextureSize = 2048;
            importer.filterMode = FilterMode.Bilinear;
            importer.mipmapEnabled = false;
            importer.isReadable = false;
            TextureImporterFormat format = TextureImporterFormat.AutomaticCompressed;
            //if (GetFolderQualitySetting(folderPath) == QUALITY_TURE_COLOR)
            {
                //format = TextureImporterFormat.AutomaticTruecolor;
            }
            importer.textureFormat = format;
            TextureImporterSettings setting = new TextureImporterSettings();
            importer.ReadTextureSettings(setting);
            importer.SetTextureSettings(setting);
        }

        private static void Read9SliceParam(string jsonPath)
        {
            StreamReader sr = new StreamReader(jsonPath);
            string content = sr.ReadToEnd();
            JsonData jsonData = JsonMapper.ToObject(content);
            TraversalTree(jsonData);
        }

        private static void TraversalTree(JsonData jsonData)
        {
            string typeStr = jsonData[NodeField.TYPE].ToString().ToLower();
            if (typeStr == "image")
            {
                if(jsonData.Keys.Contains(NodeField.PARAM))
                {
                    string name = jsonData[NodeField.NAME].ToString();
                    string param = jsonData[NodeField.PARAM].ToString();
                    string[] splitArray = param.Split(',');
                    Vector4 v4 = new Vector4(float.Parse(splitArray[3]), float.Parse(splitArray[2]), float.Parse(splitArray[1]), float.Parse(splitArray[0]));//上下左右
                    if(!_sliceDict.ContainsKey(name))
                    {
                        _sliceDict.Add(name, v4);
                    }
                }
            }
            if(jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                int length = jsonData[NodeField.CHILDREN].Count;
                JsonData children = jsonData[NodeField.CHILDREN];
                for(int i = 0; i < length; i++)
                {
                    TraversalTree(children[i]);
                }
            }
        }

        private static void FormatTexture(string pngName, string assetPath)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            textureImporter.isReadable = true;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = false;
            if(_sliceDict.Keys.Contains(pngName))
            {
                textureImporter.spriteBorder = _sliceDict[pngName];
            }
            AssetDatabase.ImportAsset(assetPath);
        }
    }
}
