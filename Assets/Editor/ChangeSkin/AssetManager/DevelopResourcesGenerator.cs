using System.Collections.Generic;
using System.IO;
using Tool;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class DevelopResourcesGenerator
    {
        public static void Generate(string jsonPath)
        {
            string jsonFileName = FileUtility.GetFileName(jsonPath);
            string imageDir = FileUtility.PSD_IMAGE_DIR + FileUtility.RemovePostfix(jsonFileName);
            string[] guidList = AssetDatabase.FindAssets("t:texture2D", new string[] { imageDir });
            string moveDir = FileUtility.TEXTURE_MODULE_ATLAS_DIR + FileUtility.RemovePostfix(jsonFileName) + FileUtility.XIUXIAN_POSTFIX + "_/";
            FileUtility.CreateDirectory(moveDir);

            DeleteExistTexture(guidList, moveDir);
            CopySourceTexture(guidList, jsonPath, moveDir);
            CreateRelativePrefab(jsonFileName, moveDir);
        }

        private static void DeleteExistTexture(string[] guidList, string moveDir)
        {
            for(int i = 0; i < guidList.Length; i++)
            {
                string guid = guidList[i];
                string sourcePath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = FileUtility.GetFileName(sourcePath);
                string destPath = moveDir + fileName + FileUtility.PNG_POSTFIX;
                if(File.Exists(destPath))
                {
                    File.Delete(destPath);
                }
                AssetDatabase.ImportAsset(destPath);
            }
        }

        private static void CopySourceTexture(string[] guidList, string jsonPath, string moveDir)
        {
            Dictionary<string, string[]> dict = FileUtility.GetSliceDict(jsonPath);
            for(int i = 0; i < guidList.Length; i++)
            {
                string guid = guidList[i];
                string sourcePath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = FileUtility.GetFileName(sourcePath);
                string destPath = moveDir + fileName + FileUtility.PNG_POSTFIX;
                if(dict.ContainsKey(fileName))
                {
                    SliceTexutre(sourcePath, dict[fileName], destPath);
                    FormatTexture(sourcePath, destPath, dict[fileName]);
                }
                else
                {
                    File.Copy(sourcePath, destPath);
                    AssetDatabase.ImportAsset(destPath);
                    FormatTexture(sourcePath, destPath, null);
                }
                AssetDatabase.ImportAsset(destPath);
            }
        }

        private static void FormatTexture(string sourcePath, string destPath, string[] sliceParam)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(destPath) as TextureImporter;
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            textureImporter.alphaIsTransparency = true;
            textureImporter.isReadable = false;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.spritePixelsPerUnit = 1;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.filterMode = FilterMode.Bilinear;
            textureImporter.anisoLevel = 1;
            textureImporter.spritePivot = new Vector2(0.5f, 0.5f);
            textureImporter.mipmapEnabled = false;
            string packingTag = FileUtility.GetFolderName(destPath).Replace("_", string.Empty);
            textureImporter.spritePackingTag = packingTag;

            textureImporter.spriteBorder = Vector4.zero;
            if(sliceParam != null)
            {
                textureImporter.spriteBorder = new Vector4(float.Parse(sliceParam[3]), float.Parse(sliceParam[2]), float.Parse(sliceParam[1]), float.Parse(sliceParam[0])); 
            }

            TextureImporterSettings settings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            textureImporter.SetTextureSettings(settings);

            Texture2D texture = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(Texture2D)) as Texture2D;
            int maxSize = GetMaxSize(texture);
            textureImporter.SetPlatformTextureSettings("Standalone", maxSize, TextureImporterFormat.RGBA32);
            textureImporter.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGBA4);
            textureImporter.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC2_RGBA8);
        }

        private static void CreateRelativePrefab(string imageFolderName, string moveDir)
        {
            string spriteDir = FileUtility.RESOURCE_SPRITE_DIR + imageFolderName + "/";
            FileUtility.RecreateDirectory(spriteDir);
            DirectoryInfo dirInfo = new DirectoryInfo(moveDir);
            foreach (FileInfo pngFile in dirInfo.GetFiles("*" + FileUtility.PNG_POSTFIX, SearchOption.TopDirectoryOnly))
            {
                string assetPath = FileUtility.AllPath2AssetPath(pngFile.FullName);
                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                string prefabPath = spriteDir + sprite.name + FileUtility.PREFAB_POSTFIX;
                PrefabUtility.CreatePrefab(prefabPath, go);
                GameObject.DestroyImmediate(go);
                AssetDatabase.ImportAsset(prefabPath);
            }
        }

        private static void SliceTexutre(string sourcePath, string[] sliceParam, string destPath)
        {
            Texture2D srcTexture = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(Texture2D)) as Texture2D;
            Texture2D destTexture = TextureSlicer.Slice(srcTexture, sliceParam);
            byte[] pngData = destTexture.EncodeToPNG();
            File.WriteAllBytes(destPath, pngData);
            AssetDatabase.ImportAsset(destPath);
        }

        private static int GetMaxSize(Texture2D texture)
        {
            int maxSize = 32;
            float size = Mathf.Max(texture.width, texture.height);
            size = Mathf.Pow(2f, Mathf.Ceil(Mathf.Log(size, 2f)));
            if (size > maxSize)
            {
                maxSize = (int)size;
            }
            return maxSize;
        }
    }
}
