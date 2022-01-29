using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopItemSpawn : MonoBehaviour
{
    private int Cost;
    private float SpawnTimeReload = 0;
    private GameObject Item;
    private WeaponSwitch weaponSwitch;
    private ObjectStorage Storage;
    public Text CostText;

    public bool IsWeapon;
    public bool IsItem;
    public bool IsPickups;

    private void Start()
    {
        Storage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();
        weaponSwitch = GameObject.FindGameObjectWithTag("PlaceForGun").GetComponent<WeaponSwitch>();

        if (IsItem)
        {
            int WeaponIndex;

            while (true)
            {
                WeaponIndex = Random.Range(0, weaponSwitch.WeaponCount);
                if (weaponSwitch.UnlockedGuns[WeaponIndex])
                {
                    break;
                }
            }

            Item = Instantiate(Storage.GiveRandomItem(2, WeaponIndex), transform.position, Quaternion.identity);
        }
        else if (IsWeapon)
        {
            Item = Instantiate(Storage.GiveRandomWeapon(), transform.position, Quaternion.identity);
        }
        else if (IsPickups)
        {
            Item = Instantiate(Storage.GiveRandomPickup(), transform.position, Quaternion.identity);
            Item.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        Item.transform.parent = gameObject.transform;

        if(IsItem)
        {
            Item.transform.GetChild(0).GetChild(0).GetComponent<ShopCostForObjects>().InShop = true;
            Cost = Item.transform.GetChild(0).GetChild(0).GetComponent<ShopCostForObjects>().MoneyCostInShop;
        }
        else
        {
            Item.GetComponent<ShopCostForObjects>().InShop = true;
            Cost = Item.GetComponent<ShopCostForObjects>().MoneyCostInShop;
        }
        CostText.text = Cost.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && SpawnTimeReload >= 1f)
        {
            if(collision.GetComponent<Player>().Coins >= Cost)
            {
                collision.GetComponent<Player>().Coins -= Cost;
                collision.GetComponent<Player>().UpdateResourcesText();

                if (IsItem)
                {
                    Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ItemGrabs(collision);
                }
                else if (IsWeapon)
                {
                    Item.GetComponent<WeaponGrab>().WeaponGrabs(collision);
                }
                else if (IsPickups)
                {
                    Item.GetComponent<Pickups>().PickupGrabs(collision);
                }

                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        SpawnTimeReload += Time.deltaTime;
    }
}
