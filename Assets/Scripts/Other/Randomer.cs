using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomer : MonoBehaviour
{
    public static GameObject GetRandomFromArray(GameObject[] array)
    {
        if (array == null || array.Length == 0)
        {
            return null;
        }

        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static GameObject GetRandomFromList(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static List<GameObject> Shufle(List<GameObject> list)
    {
        int Size = list.Count;
        List<GameObject> newList = new List<GameObject>();

        for (int i = 0; i < Size; i++)
        {
            newList.Add(list[UnityEngine.Random.Range(0, list.Count)]);
            list.Remove(newList[newList.Count]);
        }

        return newList;
    }
}