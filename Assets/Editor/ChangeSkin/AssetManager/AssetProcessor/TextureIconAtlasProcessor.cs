using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class TextureIconAtlasProcessor : BaseProcessor
    {
        private string _assetPath;
        private string _packingTag;

        public TextureIconAtlasProcessor(string path)
        {
            _assetPath = path;
            string folderName = FileUtility.GetFolderName(path);
            _packingTag = folderName.Substring(0, folderName.Length - 1);
        }

        public override void PreFormatTexture(TextureImporter textureImporter)
        {
            textureImporter.isReadable = false;
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            textureImporter.alphaIsTransparency = true;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.spritePixelsPerUnit = 1;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.filterMode = FilterMode.Bilinear;
            textureImporter.anisoLevel = 1;
            textureImporter.spritePivot = Vector2.one * 0.5f;
            textureImporter.mipmapEnabled = false;
            textureImporter.spritePackingTag = _packingTag;
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
