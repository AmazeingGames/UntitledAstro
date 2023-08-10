using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static List<T> AddMultiple<T>(this List<T> list, params T[] items)
    {
        foreach (var item in items)
        {
            list.Add(item);
        }

        return list;
    }
}
