using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс сундука с улучшениями
/// </summary>
public class ItemChest : Chest
{
    private void Start()
    {
        Factory.LinkStorage();
        Animator = GetComponent<Animator>();
    }

    public override void SpawnObjectFromChest(Collider2D collision)
    {
        GameObject Item = Instantiate(Factory.CreateRandom(Factory.BaseObjects.Item), transform.position, Quaternion.identity);
        Animator.Play("ChestDestroy");
    }
}
