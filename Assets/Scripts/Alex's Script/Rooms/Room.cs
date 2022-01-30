using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Doors")]
    public GameObject DoorUp;
    public GameObject DoorDown;
    public GameObject DoorLeft;
    public GameObject DoorRight;

    [Header("DoorLockers")]
    public GameObject DoorUpLocker;
    public GameObject DoorDownLocker;
    public GameObject DoorLeftLocker;
    public GameObject DoorRightLocker;

    [Header("PossibleDoors")]
    public bool PossibleDoorUp;
    public bool PossibleDoorDown;
    public bool PossibleDoorLeft;
    public bool PossibleDoorRight;

    [Header("ObjectsInRoom")]
    public GameObject[] Doors;
    public GameObject[] DoorBlockers;
    public Transform[] EnemySpawnPoints;
    public GameObject[] ChestsSpawnPoints;
    public GameObject[] ShopPlaceSpawnPoints;
    public GameObject[] WorkshopPlaceSpawnPoints;
    public GameObject[] Pickups;
    public GameObject[] Items;
    public GameObject[] Weapons;
    public GameObject[] LaddersPlaceSpawnPoints;
    [Space]

    public GameObject DoorEffect;
    public GameObject RewardSpawnEffect;
    public GameObject Bg;
    private ObjectStorage Objects;
    public ArrayHolder RoomClearRewards;

    [HideInInspector] public List<GameObject> EnemiesLeft;

    private bool Spawned;
    public bool RewardActive;
    public bool SpecialReward;
    private bool DoorDestroyed;
    public bool EndRoom;
    public bool NewFloorReward;

    private void Start()
    {
        Objects = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();
        Bg = GameObject.FindGameObjectWithTag("BlackBG");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !Spawned)
        {
            Spawned = true;

            EnemiesSpawn(collision);
            ChestsSpawn();
            ShopPlacesSpawn();
            PickupsSpawn();
            ItemsSpawn(collision);
            WeaponsSpawn();
            WorkshopPlacesSpawn();

            StartCoroutine(CheckEnemiess());
        }
    }

    public void EnemiesSpawn(Collider2D collision)
    {
        foreach (Transform Spawner in EnemySpawnPoints)
        {
            GameObject Enemy = Instantiate(Spawner.GetComponent<OneItemHolder>().Item, Spawner.position, Quaternion.identity);
            Enemy.transform.parent = transform;
            Enemy.GetComponent<Enemy>().Target = collision.transform;
            GetComponent<Room>().EnemiesLeft.Add(Enemy);
        }
    }

    public void ChestsSpawn()
    {
        foreach (GameObject ChestSpawnPoint in ChestsSpawnPoints)
        {
            if (ChestSpawnPoint.GetComponent<SpawnChest>().IsWeaponChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().WeaponChest, ChestSpawnPoint.transform.position, Quaternion.identity);
            }
            else if (ChestSpawnPoint.GetComponent<SpawnChest>().IsItemChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ItemChest, ChestSpawnPoint.transform.position, Quaternion.identity);
            }
            else if (ChestSpawnPoint.GetComponent<SpawnChest>().IsPickupsChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ResourceChest, ChestSpawnPoint.transform.position, Quaternion.identity);
            }
        }
    }

    public void ShopPlacesSpawn()
    {
        foreach (GameObject shopPlaceSpawnPoint in ShopPlaceSpawnPoints)
        {
            GameObject NewShopPlace = Instantiate(shopPlaceSpawnPoint.GetComponent<ShopPlaceSpawnPoint>().ShopPlace, shopPlaceSpawnPoint.transform.position, Quaternion.identity);

            if (shopPlaceSpawnPoint.GetComponent<ShopPlaceSpawnPoint>().IsWeaponPlace)
            {
                NewShopPlace.GetComponentInChildren<ShopItemSpawn>().IsWeapon = true;
            }
            else if (shopPlaceSpawnPoint.GetComponent<ShopPlaceSpawnPoint>().IsItemPlace)
            {
                NewShopPlace.GetComponentInChildren<ShopItemSpawn>().IsItem = true;
            }
            else if (shopPlaceSpawnPoint.GetComponent<ShopPlaceSpawnPoint>().IsPickupsPlace)
            {
                NewShopPlace.GetComponentInChildren<ShopItemSpawn>().IsPickups = true;
            }
        }
    }

    public void PickupsSpawn()
    {
        foreach (GameObject Pickup in Pickups)
        {
            if (Pickup.GetComponent<OneItemHolder>().Item == null)
            {
                Instantiate(Objects.GiveRandomPickup(), Pickup.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(Pickup.GetComponent<OneItemHolder>().Item, Pickup.transform.position, Quaternion.identity);
            }
        }
    }

    public void ItemsSpawn(Collider2D collision)
    {
        foreach (GameObject Item in Items)
        {
            int WeaponIndex = 0;
            int i = 0;
            while (true && i < 500)
            {
                WeaponIndex = Random.Range(0, collision.GetComponentInChildren<WeaponSwitch>().WeaponCount);
                i++;

                if (collision.GetComponentInChildren<WeaponSwitch>().UnlockedGuns[WeaponIndex])
                {
                    break;
                }
            }

            GameObject item;
            if (Item.GetComponent<OneItemHolder>().Item == null)
            {
                item = Instantiate(Objects.GiveRandomItem(2, WeaponIndex), Item.transform.position, Quaternion.identity);
            }
            else
            {
                item = Instantiate(Item.GetComponent<OneItemHolder>().Item, Item.transform.position, Quaternion.identity);
            }
            item.transform.parent = Item.transform.GetChild(0);
        }
    }

    public void WeaponsSpawn()
    {
        foreach (GameObject Weapon in Weapons)
        {
            GameObject Item;
            if (Weapon.GetComponent<OneItemHolder>().Item == null)
            {
                Item = Instantiate(Objects.GiveRandomWeapon(), Weapon.transform.position, Quaternion.identity);
            }
            else
            {
                Item = Instantiate(Weapon.GetComponent<OneItemHolder>().Item, Weapon.transform.position, Quaternion.identity);
            }
            Item.transform.parent = Weapon.transform.GetChild(0);
        }
    }

    public void WorkshopPlacesSpawn()
    {
        foreach (GameObject Place in WorkshopPlaceSpawnPoints)
        {
            Instantiate(Place.GetComponent<OneItemHolder>().Item, Place.transform.position, Quaternion.identity);
        }
    }

    public void LaddersPlacesSpawn()
    {
        foreach (GameObject Place in LaddersPlaceSpawnPoints)
        {
            Instantiate(Place.GetComponent<OneItemHolder>().Item, Place.transform.position, Quaternion.identity);
        }
    }

    IEnumerator CheckEnemiess()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => EnemiesLeft.Count == 0);
        if(!DoorDestroyed)
        {
            DestroyDoors();
            StartCoroutine(SpawnReward());
            if (NewFloorReward)
            {
                LaddersPlacesSpawn();
            }
            if(EndRoom)
            {
                Bg.GetComponent<Animator>().Play("GameComplete");
            }
        }
    }

    private void DestroyDoors()
    {
        foreach(GameObject DoorBlocker in DoorBlockers)
        {
            DoorBlocker.SetActive(false);
        }
        foreach (GameObject Door in Doors)
        {
            Door.GetComponent<Animator>().Play("DoorOpen");
        }
        DoorDestroyed = true;
    }

    private IEnumerator SpawnReward()
    {

        if(RewardActive)
        {
            Instantiate(RewardSpawnEffect, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            if (SpecialReward)
            {
                //Instantiate(Objects.GiveItem, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(RoomClearRewards.Items[Random.Range(0, RoomClearRewards.Items.Length)], transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            }
            RewardActive = false;
        }
    }

}
