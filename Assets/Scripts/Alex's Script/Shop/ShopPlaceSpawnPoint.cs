using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlaceSpawnPoint : MonoBehaviour
{
    public GameObject ShopPlace;

    public bool IsWeaponPlace;
    public bool IsItemPlace;
    public bool IsPickupsPlace;

    public void Spawn()
    {
        GameObject NewShopPlace = Instantiate(ShopPlace, transform.position, Quaternion.identity);

        if (IsWeaponPlace)
        {
            NewShopPlace.GetComponent<ShopItemSpawn>().IsWeapon = true;
        }
        else if (IsItemPlace)
        {
            NewShopPlace.GetComponent<ShopItemSpawn>().IsItem = true;
        }
        else if (IsPickupsPlace)
        {
            NewShopPlace.GetComponent<ShopItemSpawn>().IsPickups = true;
        }
    }
}
