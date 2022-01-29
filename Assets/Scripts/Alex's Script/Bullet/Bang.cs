using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bang : MonoBehaviour
{
    public float BangRadius;
    public float Damage;
    private float LifeTime = 0.1f;

    void Start()
    {
        GetComponent<CircleCollider2D>().radius = BangRadius;
    }

    void Update()
    {
        if(LifeTime < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            LifeTime -= Time.deltaTime;
        }
    }
}
