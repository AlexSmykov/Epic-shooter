using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorkshopItemSpawn : MonoBehaviour
{
    private int Cost;
    private float SpawnTimeReload = 0;
    private GameObject Item;
    private ObjectStorage Storage;
    public Text CostText;

    private void Start()
    {
        Storage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();

        int i = 0;
        while(true && i < 500)
        {
            i++;
            Item = Storage.Items.Items[Random.Range(0, Storage.Items.Items.Length)];

            if(Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState == 1)
            {
                break;
            }
        }

        if(Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState != 1)
        {
            foreach(GameObject item in Storage.Items.Items)
            {
                if(item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState == 1)
                {
                    Item = item;
                    break;
                }
            }
            
            if(Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState != 1)
            {
                Item = null;
                Debug.Log("delete");
                Destroy(gameObject);
                return;
            }

        }
        GameObject NewItem = Instantiate(Item, transform.position, Quaternion.identity);
        NewItem.transform.parent = gameObject.transform;

        NewItem.transform.GetChild(0).GetChild(0).GetComponent<ShopCostForObjects>().InShop = true;
        Cost = NewItem.transform.GetChild(0).GetChild(0).GetComponent<ShopCostForObjects>().MaterialsCostInWorkshop;
        CostText.text = Cost.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && SpawnTimeReload >= 1f)
        {
            if (collision.GetComponent<Player>().Materials >= Cost)
            {
                collision.GetComponent<Player>().Materials -= Cost;
                collision.GetComponent<Player>().UpdateResourcesText();

                Item.transform.GetChild(0).GetChild(0).GetComponent<Item>().ActivityState = 2;
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        SpawnTimeReload += Time.deltaTime;
    }
}