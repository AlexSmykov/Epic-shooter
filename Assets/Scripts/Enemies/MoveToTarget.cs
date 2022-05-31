using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public float Speed;
    public float WaitTime;
    private Transform Target;
    private Animator _Animator;

    public Transform TargetPlayer
    {
        get { return Target; }

        set
        {
            Target = value;
        }
    }

    private void Start()
    {
        Target = GetComponent<Enemy>().Target;
        _Animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        WaitTime -= Time.fixedDeltaTime;
        if(WaitTime <= 0)
        {
            _Animator.SetBool("IsEnemyRunning", true);
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed);
        }
    }
}
