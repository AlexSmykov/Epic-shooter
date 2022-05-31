using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для чтения txt файлов
/// </summary>
public class TxtReader : MonoBehaviour
{
    public string[] Read(string FileName)
    {
        TextAsset item = (TextAsset)Resources.Load(FileName, typeof(TextAsset));
        return item.text.Split('\n');
    }
}
