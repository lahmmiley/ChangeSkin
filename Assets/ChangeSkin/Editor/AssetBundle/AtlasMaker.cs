using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Tool
{
    public class AtlasMaker
    {
        [MenuItem("MyMenu/AtlasMaker")]
        static private void MakeAtlas()
        {
            string spriteDir = Application.dataPath + "/Atlas/";

            if (!Directory.Exists(spriteDir))
            {
                Directory.CreateDirectory(spriteDir);
            }

            Texture2D atlas = new Texture2D(2048, 2048);
            List<Texture2D> list = new List<Texture2D>();

            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/UI/Image/Common/");
            //foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                foreach (FileInfo pngFile in rootDirInfo.GetFiles("*.png", SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    Texture2D texture = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
                    //Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    //Debug.LogError(assetPath);
                    //GameObject go = new GameObject(sprite.name);
                    //go.AddComponent<SpriteRenderer>().sprite = sprite;
                    //allPath = spriteDir + "/" + sprite.name + ".prefab";
                    //string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    //PrefabUtility.CreatePrefab(prefabPath, go);
                    //GameObject.DestroyImmediate(go);

                    //Texture2D sprite = Resources.Load(
                    //    string.Format("IMAGE/{0}/{1}", PanelCreator.Instance.CurrentName, stateDict[state]),
                    //    typeof(Sprite)) as Sprite;
                    list.Add(texture);
                }
            }
            Rect[] rects = atlas.PackTextures(list.ToArray(), 5);
            atlas = AtlasOptimizer.OptimizeAtlas(atlas, rects);
            SaveTexture(atlas);

            string pngPath = Application.dataPath + "/Resources/b.png";
            string assetPath1 = pngPath.Substring(pngPath.IndexOf("Assets"));
            CreateMultipleModeSpriteImporter(assetPath1, rects, atlas.width, atlas.height);
            AssetDatabase.ImportAsset(assetPath1, ImportAssetOptions.ForceUncompressedImport);
        }

        private static void SaveTexture(Texture2D texture)
        {
            byte[] pngData = texture.EncodeToPNG();
            string pngPath = Application.dataPath + "/Resources/b.png";
            File.WriteAllBytes(pngPath, pngData);
            string assetPath = pngPath.Substring(pngPath.IndexOf("Assets"));
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUncompressedImport);
            CreateTextureImporter(assetPath, true);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUncompressedImport);
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

        private static void CreateMultipleModeSpriteImporter(string path, Rect[] rects, int width, int height)
        {
            Debug.LogError(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            Debug.LogError(importer);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            SpriteMetaData[] metaDatas = new SpriteMetaData[rects.Length];
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = "gg" + i.ToString(); // texturesName[i].Replace(TEMP_TOKEN, "");
                Rect rect = rects[i];
                Debug.Log(string.Format("{0} {1} {2} {3}", rect.xMin * width, rect.yMin * height,
                    rect.width * width, rect.height));
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

        [MenuItem("MyMenu/New MakeAtlas")]
        static private void NewMakeAtlas()
        {
            string spriteDir = Application.dataPath + "/Resources/Sprite";

            if (!Directory.Exists(spriteDir))
            {
                Directory.CreateDirectory(spriteDir);
            }

            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png", SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
                    GameObject go = new GameObject(sprite.name);
                    go.AddComponent<SpriteRenderer>().sprite = sprite;
                    allPath = spriteDir + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    GameObject.DestroyImmediate(go);
                }
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            Debug.LogError(AssetDatabase.GetAssetPath(texture));
        }
    }
}