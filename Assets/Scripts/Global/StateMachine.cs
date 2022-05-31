using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Скрипт для Контроля состояния активности предметов
/// </summary>
public class StateMachine : MonoBehaviour
{
    //Список оружий
    public enum WeaponLink
    {
        None,
        Pistol,
        Shotgun,
        Machinegun,
        Rifle,
        Plasmagun,
        Bazooka,
    }

    //Список состояний активности
    public enum ItemActivityState
    {
        LockedForResearch, //Недоступно вообще никак
        AvailableToResearch, //Можно изучить в лаборатории
        LockedForUse, //Изучено, но в игре воспользоваться нельзя
        AvailableToUse, //Доступно для использования в игре
    }

    private string[] _Types = new string[4] {"Weapons", "Pickups", "Items", "Chests"};
    private TxtReader _Reader = new TxtReader();

    public struct StateMachineItem
    {
        private ItemActivityState _ActivityState;
        private WeaponLink _InConnectWithWeapon;
        private int _Id;
        private string _Name;
        private bool _IsWeapon;

        public StateMachineItem(string Name, ItemActivityState ActivityState, bool IsWeapon, WeaponLink InConnectWithWeapon, int Id)
        {
            _Name = Name;
            _ActivityState = ActivityState;
            _IsWeapon = IsWeapon;
            _InConnectWithWeapon = InConnectWithWeapon;
            _Id = Id;
        }

        public ItemActivityState ActivityState
        {
            get => _ActivityState;
            set
            {
                _ActivityState = value;
            }
        }

        public WeaponLink InConnectWithWeapon
        {
            get => _InConnectWithWeapon;
            set
            {
                _InConnectWithWeapon = value;
            }
        }

        public int Id
        {
            get => _Id;
            set
            {
                if (value < 0)
                {
                    _Id = value;
                }
            }
        }

        public string Name
        {
            get => _Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _Name = value;
                }
            }
        }

        public bool IsWeapon
        {
            get => _IsWeapon;
            set
            {
                _IsWeapon = value;
            }
        }
    }

    private List<StateMachineItem> _Items = new List<StateMachineItem>();
    private GameObject _Storages;

    void Start()
    {
        _Storages = GameObject.Find("AllStorages");
        SetItems();
    }

    public StateMachineItem GetItemById(int Id)
    {
        if (_Items.Count > Id && Id >= 0)
        {
            return _Items[Id];
        }

        return new StateMachineItem();
    }

    public StateMachineItem GetItemByName(string Name)
    {
        foreach (StateMachineItem item in _Items)
        {
            if (item.Name == Name)
            {
                return item;
            }
        }

        return new StateMachineItem();
    }

    public void SetStateById(ItemActivityState State, int Id)
    {
        StateMachineItem NewItem = new StateMachineItem(
            _Items[Id].Name,
            State,
            _Items[Id].IsWeapon,
            _Items[Id].InConnectWithWeapon,
            Id);

        _Items.Remove(_Items[Id]);
        _Items.Insert(Id, NewItem);
    }

    public void WeaponsUnlocked(WeaponLink weapon)
    {
        foreach (StateMachineItem item in _Items)
        {
            if(item.InConnectWithWeapon == weapon && item.ActivityState == ItemActivityState.LockedForUse)
            {
                SetStateById(ItemActivityState.AvailableToUse, item.Id);
            }
        }
    }

    private void SetItems()
    {
        List<string[]> textAssets = new List<string[]>();
        List<ArrayHolder> Objects = new List<ArrayHolder>();

        foreach (string Type in _Types)
        {
            textAssets.Add(_Reader.Read("ObjectActivityStates/" + Type));
            Objects.Add(_Storages.transform.Find("All" + Type).GetComponent<ArrayHolder>());
        }

        int Id = 0;
        for (int i = 0; i < Objects.Count; i++)
        {
            for (int j = 0; j < Objects[i].Items.Length; j++)
            {
                _Items.Add(new StateMachineItem(
                    textAssets[i][j].Split(';')[0],
                    (ItemActivityState)Enum.Parse(typeof(ItemActivityState), textAssets[i][j].Split(';')[1]),
                    bool.Parse(textAssets[i][j].Split(';')[2]),
                    (WeaponLink)Enum.Parse(typeof(WeaponLink), textAssets[i][j].Split(';')[3]),
                    Id));

                Objects[i].Items[j].GetComponent<WithId>().Id = Id;
                Id++;
            }
        }

        foreach(GameObject item in _Storages.transform.Find("AllChestRewards").GetComponent<ArrayHolder>().Items)
        {
            item.GetComponent<WithId>().Id = GetItemByName(item.name).Id;
        }
    }
}