using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEditor;

namespace AssetManager
{
    public class UITextureProcessor : PsdImageProcessor
    {
        private string _folderName;
        public UITextureProcessor(string path)
        {
            assetPath = path;
            assetName = FileUtility.GetFileName(path);
            string folderName = FileUtility.GetFolderName(path);
            _folderName = folderName;
            jsonName = folderName.Substring(0, folderName.Length - 3);
            jsonPath = FileUtility.GetJsonPath(jsonName);
        }

        protected override void SetPackingTag(TextureImporter textureImporter)
        {
            textureImporter.spritePackingTag = _folderName;
        }

        protected override bool Readable()
        {
            return false;
        }
    }
}
