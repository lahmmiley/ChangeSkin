using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssetManager
{
    public class TextureSlicer
    {
        public static Texture2D Slice(Texture2D srcTexture, string[] sliceParam)
        {
            int srcWidth = srcTexture.width;
            int srcHeight = srcTexture.height;
            int left = int.Parse(sliceParam[3]);
            int right = int.Parse(sliceParam[1]);
            int top = int.Parse(sliceParam[0]);
            int bottom = int.Parse(sliceParam[2]);
            CheckBound(left, right, top, bottom);
            int destWidth = GetTarWidth(srcWidth, left, right);
            int destHeight = GetTarHeight(srcHeight, top, bottom);

            Texture2D destTexture = new Texture2D(destWidth, destHeight);
            if(HorizontalSlice(left, right) && (VerticalSlice(top, bottom)))
            {
                SetPixels(destTexture, 0, 0, srcTexture, 0, 0, left, bottom);//下左
                SetPixels(destTexture, left, 0, srcTexture, left, 0, 1, bottom);//下中
                SetPixels(destTexture, left + 1, 0, srcTexture, srcWidth - right, 0, right, bottom);//下右

                SetPixels(destTexture, 0, bottom, srcTexture, 0, bottom, left, 1);//中左
                SetPixels(destTexture, left, bottom, srcTexture, left, bottom, 1, 1);//中中
                SetPixels(destTexture, left + 1, bottom, srcTexture, srcWidth - right, bottom, right, 1);//中右

                SetPixels(destTexture, 0, bottom + 1, srcTexture, 0, srcHeight - top, left, top);//上左
                SetPixels(destTexture, left, bottom + 1, srcTexture, left, srcHeight - top, 1, top);//上中
                SetPixels(destTexture, left + 1, bottom + 1, srcTexture, srcWidth - right, srcHeight - top, right, top);//上右
            }
            else if(HorizontalSlice(left, right))
            {
                SetPixels(destTexture, 0, 0, srcTexture, 0, 0, left + 1, destHeight);//左 和 中
                SetPixels(destTexture, left + 1, 0, srcTexture, srcWidth - right, 0, right, destHeight);//右
            }
            else if(VerticalSlice(top, bottom))
            {
                SetPixels(destTexture, 0, 0, srcTexture, 0, 0, destWidth, bottom + 1);//下 和 中
                SetPixels(destTexture, 0, bottom + 1, srcTexture, 0, srcHeight - top, destWidth, top);//上
            }

            return destTexture;
        }

        private static void CheckBound(int left, int right, int top, int bottom)
        {
            if(left * right == 0)
            {
                if((left != 0) || (right != 0))
                {
                    throw new Exception("目前不支持 left right 只有一个为0");
                }
            }

            if(top * bottom == 0)
            {
                if((top != 0) || (bottom != 0))
                {
                    throw new Exception("目前不支持 top bottom 只有一个为0");
                }
            }
        }

        private static int GetTarHeight(int srcHeight, int top, int bottom)
        {
            if(VerticalSlice(top, bottom))
            {
                return 1 + top + bottom;
            }
            return srcHeight;
        }

        private static int GetTarWidth(int srcWidth, int left, int right)
        {
            if(HorizontalSlice(left, right))
            {
                return 1 + left + right;
            }
            return srcWidth;
        }

        private static bool HorizontalSlice(int left, int right)
        {
            return left != 0 && right != 0;
        }

        private static bool VerticalSlice(int top, int bottom)
        {
            return top != 0 && bottom != 0;
        }

        private static void SetPixels(Texture2D destTexture, int destPositionX, int destPositionY, Texture2D srcTexture, int srcPositionX, int srcPositionY, int width, int height)
        {
            destTexture.SetPixels(destPositionX, destPositionY, width, height, srcTexture.GetPixels(srcPositionX, srcPositionY, width, height));
        }
    }
}
