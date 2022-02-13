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

    [Header("PossibleDoors")]
    public bool PossibleDoorUp;
    public bool PossibleDoorDown;
    public bool PossibleDoorLeft;
    public bool PossibleDoorRight;

    public GameObject[] Doors;
    public List<List<Transform>> AllObjectsInRoom = new List<List<Transform>>();
    [Space]

    public GameObject DoorEffect;
    public GameObject RewardSpawnEffect;
    private GameObject Bg;
    private ObjectStorage Objects;
    public ArrayHolder RoomClearRewards;
    public GameObject RoomObjects;

    [HideInInspector] public List<GameObject> EnemiesLeft;

    private bool Spawned;
    public bool ClearReward;
    public bool ClearSpecialReward;
    private bool DoorDestroyed;
    public bool EndRoom;
    public bool NewFloorReward;

    private void Start()
    {
        Objects = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();
        Bg = GameObject.FindGameObjectWithTag("BlackBG");
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void RoomObjectsDeserealization()
    {
        for(int i = 0; i < RoomObjects.transform.childCount; i++)
        {
            List<Transform> row = new List<Transform>();
            for (int j = 0; j < RoomObjects.transform.GetChild(i).childCount; j++)
            {
                row.Add(RoomObjects.transform.GetChild(i).GetChild(j));
            }
            AllObjectsInRoom.Add(row);
        } 
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !Spawned)
        {
            RoomObjectsDeserealization();
            Spawned = true;
            if (AllObjectsInRoom[0].Count > 0) { EnemiesSpawn(collision); }
            if (AllObjectsInRoom[1].Count > 0) { ChestsSpawn(); }
            if (AllObjectsInRoom[2].Count > 0) { ShopPlacesSpawn(); }
            if (AllObjectsInRoom[3].Count > 0) { WorkshopPlacesSpawn(); }
            if (AllObjectsInRoom[4].Count > 0) { PickupsSpawn(); }
            if (AllObjectsInRoom[5].Count > 0) { ItemsSpawn(collision); }
            if (AllObjectsInRoom[6].Count > 0) { WeaponsSpawn(); }

            StartCoroutine(CheckEnemiess());
        }
    }

    public void EnemiesSpawn(Collider2D collision)
    {
        foreach (Transform Spawner in AllObjectsInRoom[0])
        {
            GameObject Enemy = Instantiate(Spawner.GetComponent<OneItemHolder>().Item, Spawner.position, Quaternion.identity);
            Enemy.transform.parent = transform;
            Enemy.GetComponent<Enemy>().Target = collision.transform;
            GetComponent<Room>().EnemiesLeft.Add(Enemy);
        }
    }

    [System.Obsolete]
    public void ChestsSpawn()
    {
        foreach (Transform ChestSpawnPoint in AllObjectsInRoom[1])
        {
            if (ChestSpawnPoint.GetComponent<SpawnChest>().IsWeaponChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().WeaponChest, ChestSpawnPoint.position, Quaternion.identity);
            }
            else if (ChestSpawnPoint.GetComponent<SpawnChest>().IsItemChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ItemChest, ChestSpawnPoint.position, Quaternion.identity);
            }
            else if (ChestSpawnPoint.GetComponent<SpawnChest>().IsPickupsChest)
            {
                Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ResourceChest, ChestSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                int index = Random.RandomRange(0, 3);

                if (index == 0)
                {
                    Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().WeaponChest, ChestSpawnPoint.position, Quaternion.identity);
                }
                else if (index == 1)
                {
                    Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ItemChest, ChestSpawnPoint.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(ChestSpawnPoint.GetComponent<SpawnChest>().ResourceChest, ChestSpawnPoint.position, Quaternion.identity);
                }
            }
        }
    }

    [System.Obsolete]
    public void ShopPlacesSpawn()
    {
        foreach (Transform shopPlaceSpawnPoint in AllObjectsInRoom[2])
        {
            shopPlaceSpawnPoint.GetComponent<ShopPlaceSpawnPoint>().Spawn();
        }
    }
    public void WorkshopPlacesSpawn()
    {
        foreach (Transform Place in AllObjectsInRoom[3])
        {
            Instantiate(Place.GetComponent<OneItemHolder>().Item, Place.position, Quaternion.identity);
        }
    }

    public void PickupsSpawn()
    {
        foreach (Transform Pickup in AllObjectsInRoom[4])
        {
            Debug.Log(Pickup);
            Debug.Log(Pickup.GetComponent<OneItemHolder>().Item);
            if (Pickup.GetComponent<OneItemHolder>().Item == null)
            {
                Instantiate(Objects.GiveRandomPickup(), Pickup.position, Quaternion.identity);
            }
            else
            {
                Instantiate(Pickup.GetComponent<OneItemHolder>().Item, Pickup.position, Quaternion.identity);
            }
        }
    }

    public void ItemsSpawn(Collider2D collision)
    {
        foreach (Transform Item in AllObjectsInRoom[5])
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
                item = Instantiate(Objects.GiveRandomItem(2, WeaponIndex), Item.position, Quaternion.identity);
            }
            else
            {
                item = Instantiate(Item.GetComponent<OneItemHolder>().Item, Item.position, Quaternion.identity);
            }
            item.transform.parent = Item.transform.GetChild(0);
        }
    }

    public void WeaponsSpawn()
    {
        foreach (Transform Weapon in AllObjectsInRoom[6])
        {
            GameObject Item;
            if (Weapon.GetComponent<OneItemHolder>().Item == null)
            {
                Item = Instantiate(Objects.GiveRandomWeapon(), Weapon.position, Quaternion.identity);
            }
            else
            {
                Item = Instantiate(Weapon.GetComponent<OneItemHolder>().Item, Weapon.position, Quaternion.identity);
            }
            Item.transform.parent = Weapon.transform.GetChild(0);
        }
    }


    public void LaddersPlacesSpawn()
    {
        foreach (Transform Place in AllObjectsInRoom[7])
        {
            Instantiate(Place.GetComponent<OneItemHolder>().Item, Place.position, Quaternion.identity);
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
        foreach(GameObject Door in Doors)
        {
            Debug.Log(Door.GetComponent<SpriteRenderer>().sortingOrder);
            if(Door.GetComponent<SpriteRenderer>().sortingOrder == -3)
            {
                Door.GetComponent<OneItemHolder>().Item.SetActive(false);
                Door.GetComponent<Animator>().Play("DoorOpen");
            }
        }
        DoorDestroyed = true;
    }

    private IEnumerator SpawnReward()
    {

        if(ClearReward || ClearSpecialReward)
        {
            Instantiate(RewardSpawnEffect, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            if (ClearSpecialReward)
            {
                WeaponSwitch weaponSwitch = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<WeaponSwitch>();
                int WeaponIndex = 0;
                int i = 0;
                while (true && i < 500)
                {
                    WeaponIndex = Random.Range(0, weaponSwitch.WeaponCount);
                    i++;

                    if (weaponSwitch.UnlockedGuns[WeaponIndex])
                    {
                        break;
                    }
                }
                Instantiate(Objects.GiveRandomItem(2, WeaponIndex), transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(RoomClearRewards.Items[Random.Range(0, RoomClearRewards.Items.Length)], transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            }
            ClearReward = false;
        }
    }

}
