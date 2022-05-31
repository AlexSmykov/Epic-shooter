using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс сундука с оружием
/// </summary>
public class WeaponChest : Chest
{
    void Start()
    {
        Factory.LinkStorage();
        Animator = GetComponent<Animator>();
    }

    public override void SpawnObjectFromChest(Collider2D collision)
    {
        GameObject Item = Instantiate(Factory.CreateRandom(Factory.BaseObjects.Weapon), transform.position, Quaternion.identity);
        Animator.Play("ChestDestroy");
    }
}
