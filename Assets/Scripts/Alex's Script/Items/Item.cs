using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name;
    public string Description;
    public GameObject PickupEffect;
    private GameObject ItemInfo;
    [HideInInspector] public float SpawnTimeReload = 0;
    public int ItemIndex;
    public int StartActivityState = 0;
    public int ActivityState = 0;
    public int WhichWeapon = 0;

    private void Start()
    {
        ActivityState = StartActivityState;
        ItemInfo = GameObject.FindGameObjectWithTag("ItemInfo");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && SpawnTimeReload >= 1f && !GetComponent<ShopCostForObjects>().InShop)
        {
            ItemGrabs(collision);
            Destroy(gameObject);
        }
    }

    public void ItemGrabs(Collider2D collision)
    {
        GetComponent<ItemEffects>().UseEffect(collision, ItemIndex);
        ItemInfo.GetComponent<ItemInfo>().ActivateInfo(Name, Description);
        Instantiate(PickupEffect, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        SpawnTimeReload += Time.deltaTime;
    }
}
