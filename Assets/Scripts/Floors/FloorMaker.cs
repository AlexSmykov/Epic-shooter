using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Главный скрипт построенния подземелья
/// </summary>
public class FloorMaker : MonoBehaviour
{
    public bool[,] RoomsOnTheMap;
    public Factory.Rooms[,] RoomTypeOnTheMap;
    public GameObject[,] RoomsMap;
    public GameObject Player;
    private Factory _Factory = new Factory();
    private Saver _Saver;
    private OpenMap _Map;
    private BlackBG _BlackBG;

    private Vector2Int[] _indexes = new Vector2Int[4] { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };

    public int DefaultRoomCount;
    public int ShopRoomCount;
    public int WorkshopRoomCount;
    public int ChestRoomCount;
    public int BossRoomCount;
    public int CurrentFloor;
    public int MaxFloor;

    private void Start()
    {
        _Factory.LinkStorage();
        _Saver = FindObjectOfType<Saver>();

        _Map = GameObject.Find("Map").GetComponent<OpenMap>();
        _Map.Init();
        GameObject player = Instantiate(Player);
        player.GetComponent<Player>().Init();

        _BlackBG = GameObject.Find("BlackBG").GetComponent<BlackBG>();
        _BlackBG.Init();
        StartCoroutine(_BlackBG.ToLight());

        Vector2Int FloorSize = _Map.MapSize;
        RoomsOnTheMap = new bool[FloorSize.x, FloorSize.y];
        RoomTypeOnTheMap = new Factory.Rooms[FloorSize.x, FloorSize.y];
        RoomsMap = new GameObject[FloorSize.x, FloorSize.y];

        bool IsNewGame = bool.Parse(PlayerPrefs.GetString("IsNewGame", "true"));

        CurrentFloor = PlayerPrefs.GetInt("CurrentFloor", 1);
        GameObject.Find("CurrentFloorText").GetComponent<Text>().text =
            "Current floor: " + CurrentFloor.ToString() + "/" + MaxFloor.ToString();

        //Если подземелье новое, или загрузка старого
        if (!IsNewGame && _Saver.SaveCheck())
        {
            List<GameObject> Rooms = _Saver.Load();
            foreach (GameObject room in Rooms)
            {
                int x = (int)(room.transform.position.x / 24) + 12;
                int y = (int)(room.transform.position.y / 14) + 12;
                RoomsMap[x, y] = room;

                _Map.CheckRoom(room.GetComponent<Room>());
                _Saver.AddRoom(room);

                RoomsOnTheMap[x, y] = true;
                RoomTypeOnTheMap[x, y] = room.GetComponent<Room>().RoomType;
            }
        }
        else
        {
            Vector2Int Start = new Vector2Int(FloorSize.x / 2, FloorSize.y / 2);
            RoomsOnTheMap[Start.x, Start.y] = true;
            RoomTypeOnTheMap[Start.x, Start.y] = Factory.Rooms.Start;
            RoomsMap[Start.x, Start.y] = Instantiate(_Factory.CreateRandomRoom(Factory.Rooms.Start), new Vector3(0, 0, 0), Quaternion.identity);

            _Saver.AddRoom(RoomsMap[12, 12]);

            //На всех этажах кроме последнего, спавнятся обычные комнаты, а на последнем только комната супер босса
            if (CurrentFloor <= MaxFloor)
            {
                PlayerPrefs.SetString("IsNewGame", "false");

                List<int> RoomsCount = new List<int>();
                RoomsCount.Add(Random.Range(5 + CurrentFloor, (int)(8 + CurrentFloor * 1.6)));
                RoomsCount.Add(Random.Range((int)(2 + CurrentFloor * 0.6), (int)(3 + CurrentFloor * 1.2)));
                RoomsCount.Add(1 + CurrentFloor / 2);
                RoomsCount.Add(1 + CurrentFloor / 2);
                RoomsCount.Add(1);

                List<Factory.Rooms> RoomTypes = Factory.GetRoomTypesList(new List<Factory.Rooms>{ Factory.Rooms.Start });

                for (int i = 0; i < RoomsCount.Count; i++)
                {
                    for (int j = 0; j < RoomsCount[i]; j++)
                    {
                        PlaceOneRoom(_Factory.CreateRandomRoom(RoomTypes[i]), RoomTypes[i]);
                    }
                }
            }
            else
            {
                PlaceOneRoom(_Factory.CreateRandomRoom(Factory.Rooms.SuperBoss), Factory.Rooms.SuperBoss);
            }

        }

        PlaceDoors();
        StartCoroutine(_Map.CreateMap());
    }

    public void NewFloor()
    {
        PlayerPrefs.SetString("IsNewGame", "true");
        PlayerPrefs.SetInt("CurrentFloor", PlayerPrefs.GetInt("CurrentFloor", 1) + 1);
    }

    /// <summary>
    /// Функция для размещения одной комнаты на карте, также добавляет комнаты в разные отслеживающие переменные
    /// </summary>
    private void PlaceOneRoom(GameObject Room, Factory.Rooms type)
    {
        Vector2Int NewRoom = VacantPlacesFind();

        RoomsMap[NewRoom.x, NewRoom.y] = Instantiate(
            Room, 
            new Vector3((NewRoom.x - 12) * 24, (NewRoom.y - 12) * 14, 0), 
            Quaternion.identity);

        _Saver.AddRoom(RoomsMap[NewRoom.x, NewRoom.y]);

        RoomsOnTheMap[NewRoom.x, NewRoom.y] = true;
        RoomTypeOnTheMap[NewRoom.x, NewRoom.y] = type;
    }

    /// <summary>
    /// Функция для поиска доступных для размещения мест. Возвращает случайное возможное место
    /// </summary>
    private Vector2Int VacantPlacesFind()
    {
        HashSet<Vector2Int> VacantPlaces = new HashSet<Vector2Int>();

        int MaxX = _Map.MapSize.x;
        int MaxY = _Map.MapSize.y;

        for (int x = 0; x < MaxX; x++)
        {
            for (int y = 0; y < MaxY; y++)
            {
                if (RoomsOnTheMap[x, y] == false)
                {
                    continue;
                }

                for (int i = 0; i < _indexes.Length; i++)
                {
                    if (IsRoomCanPlaces(x + _indexes[i].x, y + _indexes[i].y))
                    {
                        VacantPlaces.Add(new Vector2Int(x + _indexes[i].x, y + _indexes[i].y));
                    }
                }
            }
        }

        return VacantPlaces.ElementAt(Random.Range(0, VacantPlaces.Count));
    }

    /// <summary>
    /// Функция для проверки, можно ли поместить комнату на переданные координаты
    /// </summary>
    private bool IsRoomCanPlaces(int x, int y)
    {
        int MaxX = _Map.MapSize.x;
        int MaxY = _Map.MapSize.y;

        Vector2Int[] indx_1 = new Vector2Int[4] { new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1), new Vector2Int(1, 1) };
        Vector2Int[] indx_2 = new Vector2Int[4] { new Vector2Int(-1, -1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1) };

        for (int i = 0; i < _indexes.Length; i++)
        {
            if (x > 1 && 
                y > 1 && 
                x < MaxX - 1 && 
                y < MaxY - 1 &&
                !RoomsOnTheMap[x, y] &&
                !RoomsOnTheMap[x + _indexes[i].x, y + _indexes[i].y] &&
                !RoomsOnTheMap[x + indx_1[i].x - _indexes[i].x, y + indx_1[i].y - _indexes[i].y] &&
                !RoomsOnTheMap[x + indx_2[i].x - _indexes[i].x, y + indx_2[i].y - _indexes[i].y])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// После спавна комнат, размещаем на них двери (просто открываем закрытые текстурами двери и убираем мешающие коллайдеры)
    /// </summary>
    private void PlaceDoors()
    {
        for (int x = 1; x < RoomsOnTheMap.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < RoomsOnTheMap.GetLength(0) - 1; y++)
            {
                if (RoomsOnTheMap[x, y])
                {
                    for (int i = 0; i < _indexes.Length; i++)
                    {
                        if (RoomsOnTheMap[x + _indexes[i].x, y + _indexes[i].y])
                        {
                            Room FirstRoom = RoomsMap[x, y].GetComponent<Room>(); 
                            FirstRoom.RoomConnect();
                            FirstRoom.Doors[i].GetComponent<SpriteRenderer>().sortingOrder = -3;
                            FirstRoom.Doors[i].GetComponent<Transform>().GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = -3;

                            if (FirstRoom.Cleared)
                            {
                                FirstRoom.DestroyDoors();
                            }
                        }
                    }
                }
            }
        }
    }
}
