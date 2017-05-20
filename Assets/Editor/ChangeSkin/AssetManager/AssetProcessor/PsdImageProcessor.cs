using LitJson;
using Psd2UGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tool;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class PsdImageProcessor : BaseProcessor
    {
        class JsonFile
        {
            public string MD5;
            public Dictionary<string, string[]> SliceDict = new Dictionary<string, string[]>();

            public JsonFile(string md5)
            {
                this.MD5 = md5;
            }
        }

        private static Dictionary<string, JsonFile> _jsonFileDict = new Dictionary<string, JsonFile>();

        protected string assetPath;
        protected string assetName;
        protected string jsonName;
        protected string jsonPath;

        public PsdImageProcessor() { }

        public PsdImageProcessor(string path)
        {
            assetPath = path;
            assetName = FileUtility.GetFileName(path);
            jsonName = FileUtility.GetFolderName(path);
            jsonPath = FileUtility.GetJsonPath(jsonName);
        }

        public override void PreFormatTexture(TextureImporter textureImporter)
        {
            textureImporter.isReadable = Readable();
            //ReadSlice();
            //textureImporter.textureType = TextureImporterType.Advanced;
            //textureImporter.npotScale = TextureImporterNPOTScale.None;
            //textureImporter.alphaIsTransparency = true;
            //textureImporter.spriteImportMode = SpriteImportMode.Single;
            //textureImporter.spritePixelsPerUnit = 1;
            //textureImporter.wrapMode = TextureWrapMode.Clamp;
            //textureImporter.filterMode = FilterMode.Bilinear;
            //textureImporter.anisoLevel = 1;
            //textureImporter.spritePivot = new Vector2(0.5f, 0.5f);
            //textureImporter.mipmapEnabled = false;
            //SetPackingTag(textureImporter);

            //if (_jsonFileDict.ContainsKey(jsonName))
            //{
            //    Dictionary<string, string[]> sliceDict = _jsonFileDict[jsonName].SliceDict;
            //    if (sliceDict.ContainsKey(assetName))
            //    {
            //        string[] param = sliceDict[assetName];
            //        textureImporter.spriteBorder = new Vector4(float.Parse(param[3]), float.Parse(param[2]), float.Parse(param[1]), float.Parse(param[0])); 
            //    }
            //}
        }

        protected virtual void SetPackingTag(TextureImporter textureImporter)
        {
            textureImporter.spritePackingTag = string.Empty;
        }

        protected virtual bool Readable()
        {
            return true;
        }

        private void ReadSlice()
        {
            string md5 = Md5Utility.GetMD5FromFile(jsonPath);
            if (_jsonFileDict.ContainsKey(jsonName))
            {
                if (_jsonFileDict[jsonName].MD5.Equals(md5))
                {
                    return;
                }
                else
                {
                    _jsonFileDict.Remove(jsonName);
                }
            }
            JsonFile jsonFile = new JsonFile(md5);
            _jsonFileDict.Add(jsonName, jsonFile);
            jsonFile.SliceDict = FileUtility.GetSliceDict(jsonPath);
        }

        public override void PostFormatTexture(Texture2D texture, TextureImporter textureImporter)
        {
            TextureImporterSettings settings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            int maxSize = 32;
            float size = Mathf.Max(texture.width, texture.height);
            size = Mathf.Pow(2f, Mathf.Ceil(Mathf.Log(size, 2f)));
            if (size > maxSize)
            {
                maxSize = (int)size;
            }
            textureImporter.SetPlatformTextureSettings("Default", maxSize, TextureImporterFormat.RGBA32);
            textureImporter.SetPlatformTextureSettings("Standalone", maxSize, TextureImporterFormat.RGBA32);
            textureImporter.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGBA4);
            textureImporter.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC2_RGBA8);
            textureImporter.SetTextureSettings(settings);
        }
    }
}
