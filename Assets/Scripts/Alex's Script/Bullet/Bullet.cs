using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
	public float Speed;
	public float LifeTime;
	private float CurrentLifeTime;
	public float Distance;
	public float Damage;
	public int PenetrationCount;
	public GameObject DestroyEffect;
	public GameObject ShootEffect;
	public LayerMask Solid;

	public bool BangAfterDestroy;
	public float BangRadius;
	public GameObject Bang;
	

    private void Start()
    {
		CurrentLifeTime = 0;
		Instantiate(ShootEffect, transform);
	}

    private void FixedUpdate()
	{
		transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);

		if(CurrentLifeTime > LifeTime)
        {
			Destroy(gameObject);
        }
        else
        {
			CurrentLifeTime += Time.fixedDeltaTime;

		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if(collision.collider.IsTouchingLayers(LayerMask.GetMask("Solid")))
        {
			if(!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("Bullet"))
			{
				if (collision.collider.CompareTag("Enemy"))
				{
					collision.collider.GetComponent<Enemy>().TakeDamage(-Damage);
					PenetrationCount--;
				}
                else
				{
					Instantiate(DestroyEffect, transform.position, Quaternion.identity);
					Destroy(gameObject);
				}

				if(PenetrationCount == 0)
				{
					if(BangAfterDestroy)
                    {
						GameObject bang = Instantiate(Bang, transform.position, Quaternion.identity);
						bang.GetComponent<Bang>().BangRadius = BangRadius;
						bang.GetComponent<Bang>().Damage = Damage;
					}

					Destroy(gameObject);
				}
			}
		}
	}
}