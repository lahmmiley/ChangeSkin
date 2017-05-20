using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;

namespace AssetManager
{
    public class ProcessorFactory
    {
        public static BaseProcessor Create(string assetPath)
        {
            if(assetPath.StartsWith(FileUtility.IMAGE_DIR))
            {
                return new PsdImageProcessor(assetPath);
            }
            //if (assetPath.StartsWith(FileUtility.TEXTURE_DIR))
            //{
            //    string folderName = FileUtility.GetFolderName(assetPath);
            //    if (folderName.EndsWith(FileUtility.XIUXIAN_POSTFIX + "_"))
            //    {
            //        return new UITextureProcessor(assetPath);
            //    }
            //}
            if(assetPath.StartsWith(FileUtility.MAP_DIR))
            {
                return new MapTextureProcessor(assetPath);
            }
            return null;
        }
    }
}
