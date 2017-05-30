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
        public PsdImageProcessor(string path)
        {
        }

        public override void PreFormatTexture(TextureImporter textureImporter)
        {
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            textureImporter.mipmapEnabled = false;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.isReadable = true;
        }

        public override void PostFormatTexture(Texture2D texture, TextureImporter textureImporter)
        {
        }
    }
}
