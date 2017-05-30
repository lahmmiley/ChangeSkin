﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class MapTextureProcessor : BaseProcessor
    {
        public MapTextureProcessor(string path)
        {
        }

        public override void PreFormatTexture(TextureImporter textureImporter)
        {
            textureImporter.isReadable = false;
            textureImporter.alphaIsTransparency = false;
            textureImporter.grayscaleToAlpha = false;
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.filterMode = FilterMode.Bilinear;
            textureImporter.spriteImportMode = SpriteImportMode.None;
            textureImporter.generateCubemap = TextureImporterGenerateCubemap.None;
            textureImporter.mipmapEnabled = false;
            textureImporter.anisoLevel = 1;
            int maxSize = 256;
            textureImporter.SetPlatformTextureSettings("Standalone", maxSize, TextureImporterFormat.DXT1);
            textureImporter.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGBA4, 2);
            textureImporter.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC_RGB4, 2);
        }

        public override void PostFormatTexture(Texture2D texture, TextureImporter textureImporter)
        {
        }
    }
}