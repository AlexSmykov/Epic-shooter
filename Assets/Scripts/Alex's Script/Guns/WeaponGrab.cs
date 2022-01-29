using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrab : MonoBehaviour
{
    public int WeaponIndex;
    private float SpawnLifeTime = 0;
    public GameObject PickupEffect;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && SpawnLifeTime > 1f && !GetComponent<ShopCostForObjects>().InShop)
        {
            WeaponGrabs(collision);
            Destroy(gameObject);
        }
    }
    
    public void WeaponGrabs(Collider2D collision)
    {
        if (collision.GetComponentInChildren<WeaponSwitch>().UnlockedGuns[WeaponIndex] == true)
        {
            collision.GetComponentInChildren<Player>().Materials += Random.Range(4, 8);
            collision.GetComponentInChildren<Player>().UpdateResourcesText();
        }
        else
        {
            collision.GetComponentInChildren<WeaponSwitch>().UnlockedGuns[WeaponIndex] = true;
            collision.GetComponentInChildren<WeaponSwitch>().WeaponIndex = WeaponIndex;
        }

        Instantiate(PickupEffect, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        SpawnLifeTime += Time.deltaTime;
    }
}
