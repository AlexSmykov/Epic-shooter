using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс сундука с подбираемыми предметами
/// </summary>
public class PickupsChest : Chest
{
    void Start()
    {
        Factory.LinkStorage();
        Animator = GetComponent<Animator>();
    }

    public override void SpawnObjectFromChest(Collider2D collision)
    {
        StartCoroutine(SpawnPickups(collision.GetComponentInChildren<Player>().ChestPickupCount));
    }

    private IEnumerator SpawnPickups(int PickUpsCount)
    {
        for (int i = 0; i < Random.Range(PickUpsCount - 3, PickUpsCount + 3); i++)
        {
            GameObject NewPickup = Factory.CreateRandomChestReward(StateMachine.ItemActivityState.AvailableToUse, Factory.RewardTypes.None);
            Instantiate(
                NewPickup, 
                new Vector3(
                    transform.position.x + Random.Range(-3, 3) / 2, 
                    transform.position.y + Random.Range(-1, 1), 
                    transform.position.z), 
                Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }

        Animator.Play("ChestDestroy");
    }
}
