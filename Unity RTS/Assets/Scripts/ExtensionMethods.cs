using System;
using UnityEngine;
using System.Linq;
public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static void Populate<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }

    public static bool AllMatchValue<T>(this T[] arr, T value)
    {
        bool match = true;
        for (int i = 0; i < arr.Length; i++)
        {
            if (!arr[i].Equals(value))
                return false;
        }

        return match;
    }


    ///
    /// ALL FUNCTIONS BELOW COME FROM https://forum.unity.com/threads/how-to-get-all-components-on-an-object-that-implement-an-interface.101028/
    ///


    /// <summary>
    /// Returns all monobehaviours (casted to T)
    /// </summary>
    /// <typeparam name="T">interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfaces<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        var mObjs = gObj.GetComponents<MonoBehaviour>();

        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }

    /// <summary>
    /// Returns the first monobehaviour that is of the interface type (casted to T)
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterface<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfaces<T>().FirstOrDefault();
    }

    /// <summary>
    /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterfaceInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

        var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();

        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }
}