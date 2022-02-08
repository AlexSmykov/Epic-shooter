using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomer : MonoBehaviour
{
    public Sprite[] Variables;

    [System.Obsolete]
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Variables[Random.RandomRange(0, Variables.Length)];
    }
}
