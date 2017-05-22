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
            if(assetPath.StartsWith(FileUtility.PSD_IMAGE_DIR))
            {
                return new PsdImageProcessor(assetPath);
            }
            else if(assetPath.StartsWith(FileUtility.TEXTURE_MODULE_ATLAS_DIR))
            {
            }
            else if(assetPath.StartsWith(FileUtility.TEXTURE_DIR) &&
                !assetPath.StartsWith(FileUtility.TEXTURE_MODULE_ATLAS_DIR))
            {
                string folder = FileUtility.GetFolderName(assetPath);
                if(folder.EndsWith(FileUtility.XIUXIAN_POSTFIX + "_"))
                {
                    return new TextureIconAtlasProcessor(assetPath);
                }
            }
            else if(assetPath.StartsWith(FileUtility.MAP_DIR))
            {
                return new MapTextureProcessor(assetPath);
            }
            return null;
        }
    }
}
