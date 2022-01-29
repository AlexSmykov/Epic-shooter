using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite UnlockedChest;
    public GameObject ForObject;
    private Animator animator;
    private float SpawnLifeTime = 1;
    public bool KeyIsNeed;

    private bool ChestUnlocked;

    public bool IsItemChest;
    public bool IsWeaponChest;
    public bool IsPickupsChest;

    private ObjectStorage Objects;
    private GameObject PlaceForGun;

    private void Start()
    {
        Objects = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();
        PlaceForGun = GameObject.FindGameObjectWithTag("PlaceForGun");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !ChestUnlocked && SpawnLifeTime <= 0 && (!KeyIsNeed || collision.GetComponent<Player>().Keys > 0))
        {
            if(KeyIsNeed)
            {
                collision.GetComponent<Player>().Keys--;
                collision.GetComponent<Player>().UpdateResourcesText();
            }
            animator = GetComponent<Animator>();
            animator.SetBool("ChestOpened", true);
            ForObject.SetActive(true);
            SpawnObjectFromChest(collision);
            gameObject.GetComponent<SpriteRenderer>().sprite = UnlockedChest;
            ChestUnlocked = true;
        }
    }

    private void Update()
    {
        SpawnLifeTime -= Time.deltaTime;
    }

    public void SpawnObjectFromChest(Collider2D collision)
    {
        GameObject Item;

        if (IsItemChest)
        {
            int WeaponIndex = 0;

            int i = 0;
            while(true && i < 500)
            {
                WeaponIndex = Random.Range(0, collision.GetComponentInChildren<WeaponSwitch>().WeaponCount);
                i++;

                if (collision.GetComponentInChildren<WeaponSwitch>().UnlockedGuns[WeaponIndex])
                {
                    break;
                }
            }

            Item = Instantiate(Objects.GiveRandomItem(2, WeaponIndex), ForObject.transform.position, Quaternion.identity);
            Item.transform.parent = ForObject.transform;
        }
        else if(IsWeaponChest)
        {
            Item = Instantiate(Objects.GiveRandomWeapon(), ForObject.transform.position, Quaternion.identity);
            Item.transform.parent = ForObject.transform;
        }
        else if(IsPickupsChest)
        {
            StartCoroutine(SpawnPickups(collision.GetComponentInChildren<Player>().ChestPickupCount));
        }
    }

    //TODO
    public IEnumerator SpawnPickups(int PickUpsCount)
    {
        for (int i = 0; i < Random.Range(PickUpsCount - 3, PickUpsCount + 3); i++)
        {
            int Chooser = Random.Range(0, 100);
            GameObject Pickup;
            if (Chooser < 40)
            {
                Pickup = Instantiate(Objects.GivePickupByIndex(0), new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (Chooser < 47)
            {
                Pickup = Instantiate(Objects.GivePickupByIndex(1), new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (Chooser < 68)
            {
                Pickup = Instantiate(Objects.GivePickupByIndex(Random.Range(2, 4)), new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if(Chooser < 75)
            {
                Pickup = Instantiate(Objects.GivePickupByIndex(5), new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                int index;
                while (true)
                {
                    index = Random.Range(6, 11);
                    if(PlaceForGun.GetComponent<WeaponSwitch>().UnlockedGuns[index - 6])
                    {
                        break;
                    }
                }
                Pickup = Instantiate(Objects.GivePickupByIndex(index), new Vector3(0, 0, 0), Quaternion.identity);
            }
            Vector3 Position = new Vector3(gameObject.transform.position.x + Random.Range(-5, 5) / 2, gameObject.transform.position.y + Random.Range(-1, 1), gameObject.transform.position.z);
            Pickup.transform.position = Position;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
