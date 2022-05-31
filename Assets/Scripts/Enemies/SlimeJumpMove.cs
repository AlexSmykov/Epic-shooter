using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт передвижения врага как прыжок слайма
/// </summary>
[RequireComponent(typeof(Enemy))]
public class SlimeJumpMove : MonoBehaviour
{
    [SerializeField] private float _Speed;
    [SerializeField] private float _AnimationSpeed;
    private Transform _Target;
    private Vector2 _CurrentTarget;
    private Animator _Animator;
    private bool _IsJumping;
    [SerializeField] private GameObject _JumpEffect;
    
    private void Start()
    {
        _Target = GetComponent<Enemy>().Target;
        _Animator = GetComponent<Animator>();
        _Animator.speed = _AnimationSpeed;

    }

    private void FixedUpdate()
    {
        if(_IsJumping)
        {
            transform.position = Vector2.MoveTowards(transform.position, _CurrentTarget, _Speed);
        }
    }

    public void WhenJump()
    {
        _IsJumping = true;
        _CurrentTarget = _Target.position;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void JumpingEnd()
    {
        _IsJumping = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
        Instantiate(_JumpEffect, transform.position, Quaternion.identity);

        if(GetComponent<SpawnEnemiesAbility>() != null)
        {
            if(Random.Range(0, 100) < 15 + (1 - GetComponent<Enemy>().Health / GetComponent<Enemy>().StartHealth) * 15)
            {
                GetComponent<SpawnEnemiesAbility>().SpawnEnemies();
            }
        }

        _Animator.speed = Random.Range(80, 120) * 0.01f;
    }
}
