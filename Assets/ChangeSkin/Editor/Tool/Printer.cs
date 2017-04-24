using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class Printer
    {
        public static void Print<T>(HashSet<T> hashSet, string content = "")
        {
            foreach(T value in hashSet)
            {
                content += value.ToString() + "\n";
            }
            Print(content);
        }

        //public static void Print<T>(IList<T> ilist)
        //{
        //    Print(ilist.ToArray<T>());
        //}

        //public static void Print<T>(T[] array)
        //{
        //    string content = string.Empty;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        content += array[i].ToString() + "\n";
        //    }
        //    Print(content);
        //}

        public static void Print<T>(IEnumerable<T> enumerable, string content = "")
        {
            foreach(T value in enumerable)
            {
                content += value.ToString() + "\n";
            }
            Print(content);
        }

        public static void Print<T>(T content)
        {
            Debug.LogError(content);
        }

    }
}
