using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour
{
    public GameObject FloatingText;
    public bool IsPlayer;
    [HideInInspector] public float Damage;
    private TextMesh textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = Math.Round(Damage).ToString();

        if (IsPlayer)
        {
            if (Damage > 0)
            {
                textMesh.color = Color.green;
            }
            else
            {
                textMesh.color = Color.red;
            }
        }
    }

    public void OnAnimationOver()
    {
        Destroy(FloatingText);
    }
}
