using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс комнаты, которая отслеживает посещение игрока и спавнит предметы. После зачистки комнаты открывает двери
/// </summary>
public class Room : MonoBehaviour
{
    public enum RoomObjectType
    {
        Enemies,
        SpawnAtStart,
        SpawnAtClear,
    }

    private List<GameObject> _Doors = new List<GameObject>();
    private Saver _Saver;
    public List<GameObject> Doors
    {
        get { return _Doors; }
    }
    public Factory.Rooms RoomType;

    private GameObject _Bg;
    private Transform _RoomObjects;
    private Save _Save;
    private Factory _Factory = new Factory();

    [SerializeField] private bool _Cleared;
    public bool Cleared { get => _Cleared; }
    private bool _Spawned;

    [HideInInspector] public List<GameObject> EnemiesLeft;

    [SerializeField] private GameObject _RewardSpawnEffect;

    [SerializeField] private bool _ClearReward;
    [SerializeField] private bool _ClearSpecialReward;
    [SerializeField] private bool _EndRoom;
    [SerializeField] private bool _NewFloorReward;

    private void Start()
    {
        _Save = GameObject.Find("FloorSpawner").GetComponent<Save>();
    }

    public void RoomConnect()
    {
        foreach (Transform door in transform.GetChild(0).Find("Doors"))
        {
            _Doors.Add(door.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _RoomObjects = transform.Find("RoomObjects");
            _Saver = FindObjectOfType<Saver>();
            _Bg = GameObject.Find("BlackBG");

            if (!_Spawned)
            {
                if (!_Save)
                {
                    _Save = GameObject.Find("FloorSpawner").GetComponent<Save>();
                }
                _Save.SaveAllowed = false;

                if (_RoomObjects.Find("Enemies").childCount > 0)
                {
                    _EnemiesSpawn(collision);
                }

                foreach (Transform Place in _RoomObjects.Find("SpawnAtStart"))
                {
                    GameObject NewShopPlace = Instantiate(Place.GetComponent<OneItemHolder>().Item, Place.position, Quaternion.identity);
                    if (NewShopPlace.TryGetComponent(out ShopPlace ShopPLaceComponent))
                    {
                        ShopPLaceComponent.CreateItem();
                        _Saver.AddShopPlace(NewShopPlace);
                    }
                    else
                    {
                        _Saver.AddObject(NewShopPlace);
                    }
                    _DeleteSpawnpoints(RoomObjectType.SpawnAtStart);
                }

                _Spawned = true;

                StartCoroutine(_CheckEnemiess());
            }
            else
            {
                _Saver.Save();
            }
        }
    }

    private void _DeleteSpawnpoints(RoomObjectType type)
    {
        foreach(Transform Place in transform.Find("RoomObjects").Find(type.ToString()))
        {
            Place.gameObject.SetActive(false);  
        }
    }

    private void _EnemiesSpawn(Collider2D collision)
    {
        foreach (Transform Spawner in _RoomObjects.Find("Enemies"))
        {
            GameObject Enemy = Instantiate(Spawner.GetComponent<OneItemHolder>().Item, Spawner.position, Quaternion.identity);
            Enemy.transform.parent = transform;
            Enemy.GetComponent<Enemy>().Target = collision.transform;
            GetComponent<Room>().EnemiesLeft.Add(Enemy);
            _DeleteSpawnpoints(RoomObjectType.Enemies);
        }
    }

    /// <summary>
    /// Функция, отслеживающая состояние комнаты: убиты ли враги, или нет. По убийству, спавнятся награды (если есть) и открываюстя двери
    /// </summary>
    private IEnumerator _CheckEnemiess()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => EnemiesLeft.Count == 0);

        if (!_Cleared)
        {
            StartCoroutine(_SpawnReward());

            if (_NewFloorReward)
            {
                foreach (Transform Place in _RoomObjects.Find("SpawnAtClear"))
                {
                    GameObject item = Place.GetComponent<OneItemHolder>().Item;
                    _Saver.AddObject(Instantiate(item, Place.position, Quaternion.identity));
                    _DeleteSpawnpoints(RoomObjectType.SpawnAtClear);
                }
            }

            if(_EndRoom)
            {
                _Bg.GetComponent<Animator>().Play("GameComplete");
            }

            _Cleared = true;

            _Save.SaveAllowed = true;
            _Save.SaveGame(false);
            _Saver.Save();
        }
    }

    public void DestroyDoors()
    {
        if (_Doors.Count == 0)
        {
            RoomConnect();
        }

        foreach(GameObject Door in _Doors)
        {
            if (Door.GetComponent<SpriteRenderer>().sortingOrder == -3)
            {
                Door.transform.Find("DoorBlocker").gameObject.SetActive(false);
                Door.GetComponent<Animator>().Play("DoorOpen");

            }
        }
    }

    private IEnumerator _SpawnReward()
    {
        if(_ClearReward || _ClearSpecialReward)
        {
            Instantiate(_RewardSpawnEffect, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);

            if (_ClearSpecialReward)
            {
                _Saver.AddObject(Instantiate(_Factory.CreateRandom(Factory.BaseObjects.Item), transform.position + new Vector3(0, -1, 0), Quaternion.identity));
            }
            else
            {
                _Saver.AddObject(Instantiate(
                    _Factory.CreateRandomRoomReward(StateMachine.ItemActivityState.AvailableToUse, Factory.RewardTypes.None),
                    transform.position,
                    Quaternion.identity));
            }

            _ClearReward = false;
        }

        DestroyDoors();

        _Saver.Save();
    }

    /// <summary>
    /// Функция зачистки комнаты, если она была загружена, а не создана
    /// </summary>
    public void InstantClearRoom()
    {
        _DeleteSpawnpoints(RoomObjectType.Enemies);
        _DeleteSpawnpoints(RoomObjectType.SpawnAtStart);
        _DeleteSpawnpoints(RoomObjectType.SpawnAtClear);
        _Cleared = true;
        _Spawned = true;
    }
}