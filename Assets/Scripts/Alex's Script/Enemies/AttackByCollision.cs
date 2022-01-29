using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByCollision : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
            if (collision.collider.CompareTag("Player") && GetComponent<Enemy>().CurrentReloadTime <= 0)
            {
                GetComponent<Enemy>().Target.GetComponent<Player>().HealthChange(-GetComponent<Enemy>().Damage);
                GetComponent<Enemy>().CurrentReloadTime = GetComponent<Enemy>().ReloadTime;
            }
        }
    }
}
