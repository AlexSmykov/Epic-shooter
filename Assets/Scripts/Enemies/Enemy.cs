using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт врага, его ходьба, жизни и другие параметры
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private protected float _StartHealth;
    public float StartHealth
    {
        get { return _StartHealth; }
        set
        {
            if (value > 0)
            {
                _StartHealth = value;
            }
            else
            {
                _StartHealth = 0;
            }
        }
    }
    private protected float _Health;
    public float Health
    {
        get { return _Health; }
        set
        {
            if (value > 0)
            {
                _Health = value;
            }
            else
            {
                _Health = 0;
            }
        }
    }
    [SerializeField] private protected float _Damage;
    public float Damage { get { return _Damage; } set { _Damage = value; } }
    [SerializeField] protected float _ReloadTime;
    public float ReloadTime { get { return _ReloadTime; } set { _ReloadTime = value; } }
    private protected float _CurrentReloadTime;
    public float CurrentReloadTime { get { return _CurrentReloadTime; } set { _CurrentReloadTime = value; } }

    private protected Room _Room;
    public Room Room { get { return _Room; } }
    private protected Animator _Animator;
    public Animator Animator { get { return _Animator; }}
    private protected Transform _Target;
    public Transform Target { get { return _Target; } set { _Target = value; } }
    private protected SpawnEnemiesAbility _SpawnEnemiesAbility;
    private protected SpawnEnemiesAfterDieAbility _SpawnEnemiesAfterDieAbility;

    [SerializeField] private protected GameObject _FloatingDamage;
    [SerializeField] private protected GameObject _BloodEffect;
    [SerializeField] private protected GameObject _DeadEffect;
    [SerializeField] private protected GameObject _BloodByDeath;

    private void Start()
    {
        Init();
    }

    private protected void Init()
    {
        if (_Target == null)
        {
            _Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Health = StartHealth;
        _Animator = GetComponent<Animator>();
        _Room = GetComponentInParent<Room>();
        _SpawnEnemiesAbility = GetComponent<SpawnEnemiesAbility>();
        _SpawnEnemiesAfterDieAbility = GetComponent<SpawnEnemiesAfterDieAbility>();
    }

    private void Update()
    {
        OnUpdate();
    }

    private protected void OnUpdate()
    {
        _CurrentReloadTime -= Time.deltaTime;

        if (Health <= 0)
        {
            if (_SpawnEnemiesAfterDieAbility != null)
            {
                _SpawnEnemiesAfterDieAbility.OnDeath();
            }

            if (_Room != null)
            {
                _Room.EnemiesLeft.Remove(gameObject);
            }

            Instantiate(_DeadEffect, transform.position, Quaternion.identity);
            Instantiate(_BloodByDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (_Target.transform.position.x < transform.position.x)
        {
            _FlipByTarget(true);
        }
        else
        {
            _FlipByTarget(false);
        }
    }

    private protected void _FlipByTarget(bool IsRight)
    {
        if (IsRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void AfterAttackAnimation()
    {
        _Animator.SetBool("EnemyAttacks", false);
    }

    /// <summary>
    /// Выдать урон персонажу из анимации
    /// </summary>
    public void AttackAnimationDamageDeal()
    {
        Target.GetComponent<Player>().HealthChange(-Damage);
        _CurrentReloadTime = _ReloadTime;
        GetComponent<MoveToTarget>().WaitTime = 0.5f;
    }

    public virtual void TakeDamage(float Damage)
    {
        Health += Damage;
        Vector2 Randomizer = new Vector2(Random.Range(-5, 5) / 5, Random.Range(-5, 5) / 5);
        GameObject FloatDmg = Instantiate(_FloatingDamage, new Vector2(transform.position.x, transform.position.y + 0.5f) + Randomizer, Quaternion.identity);
        FloatDmg.GetComponentInChildren<FloatingDamage>().Damage = Damage;
        Instantiate(_BloodEffect, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bang"))
        {
            Debug.Log("Zadevaet");
            TakeDamage(collision.GetComponent<Bang>().Damage);
        }
    }
}
