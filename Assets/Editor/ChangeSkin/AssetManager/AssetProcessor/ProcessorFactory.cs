using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEngine;

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
            else if(assetPath.StartsWith(FileUtility.TEXTURE_DIR))
			{
				if(assetPath.StartsWith(FileUtility.MAP_DIR))
				{
					if(!assetPath.StartsWith(FileUtility.MINIMAPS_DIR))
						return new MapTextureProcessor(assetPath);
				}
				else if(assetPath.StartsWith(FileUtility.TEXTURE_MODULE_ATLAS_DIR))
				{
					
				}
				else{
					string folder = FileUtility.GetFolderName(assetPath);
					if(folder.EndsWith(FileUtility.XIUXIAN_POSTFIX + "_"))
					{
						return new TextureIconAtlasProcessor(assetPath);
					}
				}
			}
            return null;
        }
    }
}
