using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpMove : MonoBehaviour
{
    public float Speed;
    public float AnimationSpeed;
    private Transform Target;
    private Vector2 CurrentTarget;
    private Animator _Animator;
    public bool IsJumping;
    private Vector2 JumpTarget;
    public GameObject JumpEffect;
    
    private void Start()
    {
        Target = GetComponent<Enemy>().Target;
        _Animator = GetComponent<Animator>();
        _Animator.speed = AnimationSpeed;

    }

    private void FixedUpdate()
    {
        if(IsJumping)
        {
            transform.position = Vector2.MoveTowards(transform.position, CurrentTarget, Speed);
        }
    }

    public void WhenJump()
    {
        IsJumping = true;
        CurrentTarget = Target.position;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void JumpingEnd()
    {
        IsJumping = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
        Instantiate(JumpEffect, transform.position, Quaternion.identity);

        if(GetComponent<SpawnEnemiesAbility>() != null)
        {
            if(Random.Range(0, 100) < 15 + (1 - GetComponent<Enemy>().Health / GetComponent<Enemy>().StartHealth) * 15)
            {
                GetComponent<SpawnEnemiesAbility>().SpawnEnemies(2);
            }
        }
        _Animator.speed = Random.Range(90, 110) * 0.01f;
    }
}
