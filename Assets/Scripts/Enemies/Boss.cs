using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт босса. Изменений от класса врага не сильно много, например есть полоска здоровья, но в дальнейшем могут быть добавления
/// </summary>
public class Boss : Enemy
{
    [SerializeField] private GameObject _HpBarFill;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void TakeDamage(float Damage)
    {
        Health += Damage;
        Vector2 Randomizer = new Vector2(Random.Range(-5, 5) / 5, Random.Range(-5, 5) / 5);
        GameObject FloatDmg = Instantiate(_FloatingDamage, new Vector2(transform.position.x, transform.position.y + 0.5f) + Randomizer, Quaternion.identity);
        FloatDmg.GetComponentInChildren<FloatingDamage>().Damage = Damage;
        Instantiate(_BloodEffect, transform.position, Quaternion.identity);
        _HpBarFill.GetComponent<Transform>().localScale = new Vector2(_Health / StartHealth, 1);
    }
}
