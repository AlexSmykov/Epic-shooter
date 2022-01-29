using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float StartHealth;
    public float Health;
    public float Damage;
    public float ReloadTime;
    public float CurrentReloadTime;

    public Room Room;
    public Animator Animator;
    public Transform Target;

    public GameObject FloatingDamage;
    public GameObject BloodEffect;
    public GameObject DeadEffect;
    public GameObject BloodByShoot;
    public GameObject BloodByDeath;
    public GameObject HpBar;
    public GameObject HpBarFill;


    public bool IsBoss;
    public bool SpawnEnemyAfterDie;
    public int CountEnemySpawnAfterDie;

    private void Start()
    {
        Health = StartHealth;
        Animator = GetComponent<Animator>();
        Room = GetComponentInParent<Room>();
        if(!IsBoss)
        {
            HpBar.SetActive(false);
        }
    }

    void Update()
    {
        CurrentReloadTime -= Time.deltaTime;
        if (Health <= 0)
        {
            if(SpawnEnemyAfterDie && GetComponent<SpawnEnemiesAbility>() != null)
            {
                GetComponent<SpawnEnemiesAbility>().SpawnEnemies(CountEnemySpawnAfterDie);
            }
            Instantiate(DeadEffect, transform.position, Quaternion.identity);
            GameObject blood = Instantiate(BloodByDeath, transform.position, Quaternion.identity);
            GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<FloorMaker>().Trash.Add(blood);
            Room.EnemiesLeft.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void AfterAttackAnimation()
    {
        Animator.SetBool("EnemyAttacks", false);
    }

    public void AttackAnimationDamageDeal()
    {
        Target.GetComponent<Player>().HealthChange(-Damage);
        CurrentReloadTime = ReloadTime;
        GetComponent<MoveToTarget>().WaitTime = 0.5f;
    }

    public void TakeDamage(float Damage)
    {
        Health += Damage;
        Vector2 Randomizer = new Vector2(Random.Range(-5, 5) / 5, Random.Range(-5, 5) / 5);
        GameObject FloatDmg = Instantiate(FloatingDamage, new Vector2(transform.position.x, transform.position.y + 0.5f) + Randomizer, Quaternion.identity);
        FloatDmg.GetComponentInChildren<FloatingDamage>().Damage = Damage;
        Instantiate(BloodEffect, transform.position, Quaternion.identity);
        GameObject blood = Instantiate(BloodByShoot, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<FloorMaker>().Trash.Add(blood);
        if (IsBoss)
        {
            HpBarFill.GetComponent<Transform>().localScale = new Vector2(Health / StartHealth, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bang"))
        {
            TakeDamage(collision.GetComponent<Bang>().Damage);
        }
    }
}
