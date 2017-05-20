using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetManager
{
    public abstract class BaseProcessor 
    {
        public abstract void PreFormatTexture(TextureImporter textureImporter);
        public abstract void PostFormatTexture(Texture2D texture, TextureImporter textureImporter);
    }
}
