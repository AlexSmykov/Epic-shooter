using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для управления эффектом крови на земле. Должна пропадать по истечении времени
/// </summary>
public class BloodScript : MonoBehaviour
{
    const int LIFETIME = 9;
    private float LifeTime = LIFETIME;

    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, LifeTime / (LIFETIME + 3));

    }
}
