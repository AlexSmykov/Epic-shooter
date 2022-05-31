using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Скрипт для управления местами в магазине с продающимися предметами
/// </summary>
public class ShopPlace : MonoBehaviour
{
    private enum ShopType
    {
        MoneyShop,
        ResearchLab,
    }

    [SerializeField] private Sprite _ShopImage;
    [SerializeField] private Sprite _LabImage;
    [SerializeField] private ShopType _ShopType;
    private int _Cost;
    private Saver _Saver;
    private Factory _Factory = new Factory();
    private GameObject _Item;
    public string ItemName { get { return _Item.name; } }
    private bool _Loaded;
    private bool _Spawned;

    private void Start()
    {
        _Saver = GameObject.Find("Saver").GetComponent<Saver>();

        if (!_Loaded)
        {
            CreateItem();
        }
    }

    public void CreateItem()
    {
        if (!_Spawned)
        {
            Sprite Image;

            if (_ShopType == ShopType.MoneyShop)
            {
                if (_Loaded)
                {
                    _Cost = _Item.GetComponent<ShopCostForObjects>().MoneyCostInShop;
                }
                else
                {
                    _Item = _Factory.CreateRandomItem(StateMachine.ItemActivityState.AvailableToUse, StateMachine.WeaponLink.None);
                    _Cost = _Item.GetComponent<ShopCostForObjects>().MoneyCostInShop;
                }

                Image = _ShopImage;
            }
            else
            {
                if (_Loaded)
                {
                    _Cost = _Item.GetComponent<ShopCostForObjects>().MaterialsCostInWorkshop;
                }
                else
                {
                    _Item = _Factory.CreateRandomItem(StateMachine.ItemActivityState.AvailableToResearch, StateMachine.WeaponLink.None);
                    _Cost = _Item.GetComponent<ShopCostForObjects>().MaterialsCostInWorkshop;
                }
                Image = _LabImage;
            }

            GameObject NewItem = Instantiate(_Item, transform.position, Quaternion.identity, gameObject.transform);
            NewItem.GetComponent<Collider2D>().enabled = false;

            if (NewItem.TryGetComponent(out Animator animator))
            {
                animator.enabled = false;
            }

            transform.GetChild(0).Find("Cost").GetComponent<Text>().text = _Cost.ToString();
            transform.GetChild(0).Find("Cost").GetChild(0).GetComponent<Image>().sprite = Image;

            _Spawned = true;
        }
    }

    private void _OnBuy(Collider2D collision)
    {
        _Saver.DeleteShopPlace(gameObject);


        ResourcesManager resources = collision.GetComponent<ResourcesManager>();
        if (_ShopType == ShopType.ResearchLab)
        {
            if (resources.Materials >= _Cost)
            {
                resources.Materials -= _Cost;
                resources.UpdateResourcesText();
                Destroy(gameObject);
            }
        }
        else
        {
            if (resources.Coins >= _Cost)
            {
                resources.Coins -= _Cost;
                resources.UpdateResourcesText();
                Instantiate(_Item, collision.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _OnBuy(collision);
        }
    }

    public void OnLoad(GameObject LoadedItem)
    {
        _Item = LoadedItem;
        _Loaded = true;
    }
}