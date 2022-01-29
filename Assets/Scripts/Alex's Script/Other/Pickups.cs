using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public int PickupIndex;
    public int PickupCount;
    private float SpawnlifeTime = 0.3f;
    public GameObject PickupEffect;
    public GameObject PlaceForGun;

    void Start()
    {
        PlaceForGun = GameObject.FindGameObjectWithTag("PlaceForGun");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && SpawnlifeTime <= 0 && !GetComponent<ShopCostForObjects>().InShop)
        {
            PickupGrabs(collision);
            Destroy(gameObject);
        }
    }

    public void PickupGrabs(Collider2D collision)
    {
        if (PickupIndex == 0)
        {
            collision.GetComponent<Player>().Coins += PickupCount;
        }
        if (PickupIndex == 1)
        {
            collision.GetComponent<Player>().Keys += PickupCount;
        }
        if (PickupIndex >= 2 && PickupIndex <= 4)
        {
            collision.GetComponent<Player>().Materials += PickupCount;
        }
        if (PickupIndex == 5)
        {
            collision.GetComponent<Player>().HealthChange(PickupCount);
        }
        if (PickupIndex >= 6 && PickupIndex <= 11)
        {
            PlaceForGun.GetComponent<Transform>().GetChild(PickupIndex - 6).GetComponent<Gun>().BulletsCount += PickupCount;
            PlaceForGun.GetComponent<Transform>().GetChild(PickupIndex - 6).GetComponent<Gun>().BulletTextUpdate();
        }

        collision.GetComponent<Player>().UpdateResourcesText();
        Instantiate(PickupEffect, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        SpawnlifeTime -= Time.deltaTime;
    }
}
