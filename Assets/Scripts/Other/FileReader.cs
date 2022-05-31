using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileReader : MonoBehaviour
{
    public static List<string> GetArrayFromFile(string Path)
    {
        string AllText = ReadString(Path);
        string[] Strings = AllText.Split('\n');
        List<string> OutputArray = new List<string>();

        foreach (string String in Strings)
        {
            string ItemName = String.Split(';')[0];
            int Count = int.Parse(String.Split(';')[1]);

            for(int i = 0; i < Count; i++)
            {
                OutputArray.Add(ItemName);
            }
        }

        return OutputArray;
    }

    static string ReadString(string Path)
    {
        StreamReader reader = new StreamReader(Path);
        return reader.ReadToEnd();
    }
}
