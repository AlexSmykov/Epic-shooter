using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteByTarget : MonoBehaviour
{
    private Transform Target;

    private void Start()
    {
        Target = GetComponent<Enemy>().Target;
    }
    void Update()
    {
        if (Target.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            GetComponent<Enemy>().HpBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            GetComponent<Enemy>().HpBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
