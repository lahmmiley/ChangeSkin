using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using UnityEngine;

namespace Psd2UGUI
{
    public class ImageData
    {
        public string Name;
        public ImageNode.Mirror Mirror = ImageNode.Mirror.invalid;

        public ImageData(JsonData jsonData)
        {
            Name = jsonData[NodeField.NAME].ToString();
            if (jsonData.ContainKey(NodeField.MIRROR))
            {
                Mirror = (ImageNode.Mirror)Enum.Parse(typeof(ImageNode.Mirror), jsonData[NodeField.MIRROR].ToString());
            }
        }
    }
}
