using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEngine;

namespace Psd2UGUI
{
    public class ImageDataReader
    {
        private static ImageDataReader _instance;
        public static ImageDataReader Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new ImageDataReader();
                }
                return _instance;
            }
        }
        private ImageDataReader(){ }

        private Dictionary<string, ImageData> _imageDataDict;

        public void Read(string name)
        {
            JsonData jsonData = FileUtility.ReadJsonData(string.Format("{0}{1}{2}{3}", FileUtility.UI_DATA_DIR, name, FileUtility.IMAGE_DATA, FileUtility.JSON_POSTFIX));
            _imageDataDict = new Dictionary<string,ImageData>();

            Debug.LogError(jsonData.Count);
            for(int i = 0; i < jsonData.Count; i++)
            {
                ImageData imageData = new ImageData(jsonData[i]);
                _imageDataDict.Add(imageData.Name, imageData);
            }
        }

        public ImageNode.Mirror GetMirror(string name)
        {
            if(_imageDataDict.ContainsKey(name))
            {
                ImageNode.Mirror mirror = _imageDataDict[name].Mirror;
                if(mirror != ImageNode.Mirror.invalid)
                {
                    return mirror;
                }
                else
                {
                    throw new Exception(string.Format("名为{0}原始图片没有镜像参数", name));
                }
            }
            else
            {
                throw new Exception(string.Format("找不到名为{0}镜像图片的原始图片", name));
            }
        }
    }
}
