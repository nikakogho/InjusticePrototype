using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class InsideMethods
{
    public static List<T> ToList<T>(this ICollection<T> collection)
    {
        return new List<T>(collection);
    }

    public static bool Contains<T>(this ICollection<T> collection, T item)
    {
        foreach(var element in collection)
        {
            if (element.Equals(item)) return true;
        }

        return false;
    }
}
