using System;
using UnityEngine;

namespace Tool
{
    public static class StringExtention
    {
        public static Color ToColor(this string colorStr)
        {
            int r = Convert.ToInt32(colorStr.Substring(0, 2), 16);
            int g = Convert.ToInt32(colorStr.Substring(2, 2), 16);
            int b = Convert.ToInt32(colorStr.Substring(4, 2), 16);
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
