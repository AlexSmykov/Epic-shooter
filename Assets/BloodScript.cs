using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    private float LifeTime = 10;

    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, LifeTime / 10);

    }
}
