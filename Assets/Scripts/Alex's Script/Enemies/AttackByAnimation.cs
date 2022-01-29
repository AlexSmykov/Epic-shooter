using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByAnimation : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
            if (collision.collider.CompareTag("Player") && GetComponent<Enemy>().CurrentReloadTime <= 0)
            {
                GetComponent<Enemy>().Animator.SetBool("EnemyAttacks", true);
                GetComponent<Enemy>().Animator.SetBool("IsEnemyRunning", false);
            }
        }
    }
}
