using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для взрыва пули, например для ракеты
/// </summary>
public class Bang : MonoBehaviour
{
    private float _BangRadius;
    public float BangRadius
    {
        get { return _BangRadius; }
        set
        {
            if (value > 0)
            {
                _BangRadius = value;
            }
        }
    }
    private float _Damage;
    public float Damage
    { get { return _Damage; } set{ _Damage = value; } }

    private float _lifeTime = 0.1f;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = _BangRadius;
    }

    private void FixedUpdate()
    {
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }

        _lifeTime -= Time.deltaTime;
    }
}
