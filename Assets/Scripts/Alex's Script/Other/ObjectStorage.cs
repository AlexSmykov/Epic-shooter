using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : MonoBehaviour
{
    public ArrayHolder Items;
    public ArrayHolder Weapons;
    public ArrayHolder Picups;

    private GameObject Item;


    public GameObject GiveRandomItem(int ActivityIndex, int WeaponIndex)
    {
        int i = 0;
        while (true && i < 500)
        {
            Item = Items.Items[Random.Range(0, Items.Items.Length)];
            i++;

            if (Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState == ActivityIndex && 
                Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().WhichWeapon == WeaponIndex)
            {
                break;
            }
        }
        return Item;
    }

    public GameObject GiveItemByIndex(int Index)
    {
        return Items.Items[Index];
    }

    public GameObject GiveRandomWeapon()
    {
        Item = Weapons.Items[Random.Range(0, Weapons.Items.Length)];
        return Item;
    }

    public GameObject GiveWeaponByIndex(int Index)
    {
        return Weapons.Items[Index];
    }

    public GameObject GiveRandomPickup()
    {
        Item = Picups.Items[Random.Range(0, Picups.Items.Length)];
        return Item;
    }

    public GameObject GivePickupByIndex(int Index)
    {
        return Picups.Items[Index];
    }
}
