using LitJson;
using Psd2UGUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Tool;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public class TextureProcessor : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            BaseProcessor processor = ProcessorFactory.Create(assetPath);
            if(processor != null)
            {
                processor.PreFormatTexture(assetImporter as TextureImporter);
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {

            BaseProcessor processor = ProcessorFactory.Create(assetPath);
            if(processor != null)
            {
                processor.PostFormatTexture(texture, assetImporter as TextureImporter);
            }
        }
    }
}
