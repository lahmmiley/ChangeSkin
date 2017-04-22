using UnityEngine;

namespace Tool
{
    public class AtlasOptimizer
    {
        public static bool FORCE_SQUARE = true;
        ///优化Atlas
        ///1. 可设置生成正方形Atlas
        ///2.若出现超过一半面积为空的情况，删除空的部分

        public static Texture2D OptimizeAtlas(Texture2D atlas, Rect[] rects, bool forceSquare = false)
        {
            Texture2D result = atlas;
            Rect contentRect = GetAtlasContentRect(rects);
            //处理超过一半为空的情况
            if (contentRect.width <= 0.5f)
            {
                result = CreateResizeAtlas(atlas, 0.5f, 1.0f, rects);
            }
            if (contentRect.height <= 0.5f)
            {
                result = CreateResizeAtlas(atlas, 1.0f, 0.5f, rects);
            }
            if (forceSquare == true)
            {
                if (atlas.width > atlas.height)
                {
                    result = CreateResizeAtlas(atlas, 1.0f, 2.0f, rects);
                }
                else if (atlas.width < atlas.height)
                {
                    result = CreateResizeAtlas(atlas, 1.0f, 2.0f, rects);
                }
            }
            return result;
        }


        private static Texture2D CreateResizeAtlas(Texture2D atlas, float xScale, float yScale, Rect[] rects)
        {
            int width = (int)(atlas.width * xScale);
            int height = (int)(atlas.height * yScale);
            Texture2D result = new Texture2D(width, height);
            result.name = atlas.name;
            int pixelWidth = width > atlas.width ? atlas.width : width;
            int pixelHeight = height > atlas.height ? atlas.height : height;
            result.SetPixels(0, 0, pixelWidth, pixelHeight, atlas.GetPixels(0, 0, pixelWidth, pixelHeight));
            result.Apply();
            for (int i = 0; i < rects.Length; i++)
            {
                Rect rect = rects[i];
                rects[i] = new Rect(rect.xMin / xScale, rect.yMin / yScale, rect.width / xScale, rect.height / yScale);
            }
            return result;
        }

        private static Rect GetAtlasContentRect(Rect[] rects)
        {
            Rect contentRect = new Rect(0, 0, 0, 0);
            for (int i = 0; i < rects.Length; i++)
            {
                Rect rect = rects[i];
                if (rect.xMin < contentRect.xMin)
                {
                    contentRect.xMin = rect.xMin;
                }
                if (rect.yMin < contentRect.yMin)
                {
                    contentRect.yMin = rect.yMin;
                }
                if (rect.xMax > contentRect.xMax)
                {
                    contentRect.xMax = rect.xMax;
                }
                if (rect.yMax > contentRect.yMax)
                {
                    contentRect.yMax = rect.yMax;
                }
            }
            return contentRect;
        }
    }
}