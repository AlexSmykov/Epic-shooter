using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт вылетающих чисел урона
/// </summary>
public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private bool _IsPlayer;
    private float _Damage;
    public float Damage { get { return _Damage; } set { _Damage = value; } }
    private TextMesh textMesh;

    private void Start()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();
        textMesh.text = Math.Round(Damage).ToString();

        if (_IsPlayer)
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
        Destroy(gameObject);
    }
}
