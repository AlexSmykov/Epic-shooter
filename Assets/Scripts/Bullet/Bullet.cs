using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт пули с параметрами
/// </summary>
public class Bullet : MonoBehaviour
{
	private float _Speed;
	public float Speed 
	{ 
		set 
		{ 
			if (value > 0)
			{
				_Speed = value;
			}
		} 
	}
	[SerializeField] private float _LifeTime;
	private float _CurrentLifeTime;
	private float _Damage;
	public float Damage
	{
		set
		{
			_Damage = value;
		}
	}
	private int _PenetrationCount;
	public int PenetrationCount
	{
		set
		{
			if (value > 0)
			{
				_PenetrationCount = value;
			}
		}
	}
	[SerializeField] private GameObject _DestroyEffect;
	[SerializeField] private GameObject _ShootEffect;
	private Collider2D _Collider2D;

	[SerializeField] private bool _BangAfterDestroy;
	public bool BangAfterDestroy { get { return _BangAfterDestroy; } set { _BangAfterDestroy = value; } }
	[SerializeField] private float _BangRadius;
	[SerializeField] private GameObject _Bang;

	private float PenetrationReloadTime;
	
    private void Start()
    {
		_Collider2D = GetComponent<Collider2D>();
		_CurrentLifeTime = 0;
		Instantiate(_ShootEffect, transform);
	}

    private void FixedUpdate()
	{
		transform.Translate(Vector2.right * _Speed * Time.fixedDeltaTime);

		if(PenetrationReloadTime > 0)
        {
			PenetrationReloadTime -= Time.fixedDeltaTime;
		}
        else if (!_Collider2D.enabled)
		{
			_Collider2D.enabled = true;
		}

		if(_CurrentLifeTime > _LifeTime)
        {
			Destroy(gameObject);
        }
        else
        {
			_CurrentLifeTime += Time.fixedDeltaTime;
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
					collision.collider.GetComponent<Enemy>().TakeDamage(-_Damage);
					_PenetrationCount--;
					PenetrationReloadTime = 0.04f;
					_Collider2D.enabled = false;
				}
                else
				{
					Instantiate(_DestroyEffect, transform.position, Quaternion.identity);
					Destroy(gameObject);
				}

				if(_PenetrationCount == 0)
				{
					if (_BangAfterDestroy)
                    {
						GameObject bang = Instantiate(_Bang, transform.position, Quaternion.identity);
						bang.GetComponent<Bang>().BangRadius = _BangRadius;
						bang.GetComponent<Bang>().Damage = -_Damage;
					}

					Destroy(gameObject);
				}
			}
		}
	}
}