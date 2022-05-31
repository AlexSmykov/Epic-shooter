using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Дамаг от врага проходит при прикасновении
/// </summary>
[RequireComponent(typeof(Enemy))]
public class AttackByCollision : MonoBehaviour
{
    private Enemy _Enemy;

    private void Start()
    {
        _Enemy = GetComponent<Enemy>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
            if (collision.collider.CompareTag("Player") && _Enemy.CurrentReloadTime <= 0)
            {
                _Enemy.Target.GetComponent<Player>().HealthChange(-_Enemy.Damage);
                _Enemy.CurrentReloadTime = _Enemy.ReloadTime;
            }
        }
    }
}
