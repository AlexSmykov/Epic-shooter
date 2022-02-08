using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlaceSpawnPoint : MonoBehaviour
{
    public GameObject ShopPlace;

    public bool IsWeaponPlace;
    public bool IsItemPlace;
    public bool IsPickupsPlace;

    [System.Obsolete]
    public void Spawn()
    {
        GameObject NewShopPlace = Instantiate(ShopPlace, transform.position, Quaternion.identity);

        if (IsWeaponPlace)
        {
            NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsWeapon = true;
        }
        else if (IsItemPlace)
        {
            NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsItem = true;
        }
        else if (IsPickupsPlace)
        {
            NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsPickups = true;
        }
        else
        {
            int index = Random.RandomRange(0, 2);

            if(index == 0)
            {
                NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsWeapon = true;
            }
            else if (index == 1)
            {
                NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsItem = true;
            }
            else
            {
                NewShopPlace.transform.GetChild(0).GetChild(0).GetComponent<ShopItemSpawn>().IsPickups = true;
            }
        }
    }
}
