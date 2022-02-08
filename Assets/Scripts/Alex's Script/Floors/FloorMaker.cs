using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class FloorMaker : MonoBehaviour
{
    public bool[,] RoomsOnTheMap;
    public bool[,] RoomsChecked;
    public string[,] RoomTypeOnTheMap;
    public GameObject Map;
    public GameObject[,] RoomsMap;
    public ArrayHolder AllStartRooms;
    public ArrayHolder AllDefaultRooms;
    public ArrayHolder AllChestRooms;
    public ArrayHolder AllShopRooms;
    public ArrayHolder AllWorkshopRooms;
    public ArrayHolder AllBossRooms;
    public ArrayHolder AllSuperBossRooms;
    public GameObject Player;
    public GameObject Storage;

    public int DefaultRoomCount;
    public int ShopRoomCount;
    public int WorkshopRoomCount;
    public int ChestRoomCount;
    public int BossRoomCount;
    public int CurrentFloor;
    public int MaxFloor;

    private void Start()
    {
        Instantiate(Map, GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).transform);
        RoomsOnTheMap = new bool[25, 25];
        RoomsChecked = new bool[25, 25];
        RoomTypeOnTheMap = new string[25, 25];
        RoomsMap = new GameObject[25, 25];
        RoomsOnTheMap[12, 12] = true;
        RoomTypeOnTheMap[12, 12] = "Start";
        RoomsChecked[12, 12] = true;
        RoomsMap[12, 12] = Instantiate(AllStartRooms.Items[UnityEngine.Random.Range(0, AllStartRooms.Items.Length)], new Vector3(0, 0, 0), Quaternion.identity); 
        Instantiate(Storage, new Vector3(0, 0, 0), Quaternion.identity);

        CurrentFloor = PlayerPrefs.GetInt("CurrentFloor", 1);
        if(CurrentFloor <= MaxFloor)
        {
            GameObject.FindGameObjectWithTag("CurrentFloor").GetComponent<Text>().text = "Current floor: " + CurrentFloor.ToString() + "/" + MaxFloor.ToString();
            DefaultRoomCount = UnityEngine.Random.Range(5 + CurrentFloor, 8 + CurrentFloor * 2);
            for (int i = 0; i < DefaultRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllDefaultRooms.Items, "Default");
            }

            ChestRoomCount = UnityEngine.Random.Range((int)(2 + CurrentFloor * 0.6), (int)(3 + CurrentFloor * 1.2));
            for (int i = 0; i < ChestRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllChestRooms.Items, "Chest");
            }

            ShopRoomCount = 1 + CurrentFloor / 3;
            for (int i = 0; i < ShopRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllShopRooms.Items, "Shop");
            }

            WorkshopRoomCount = 1 + CurrentFloor / 3;
            for (int i = 0; i < WorkshopRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllWorkshopRooms.Items, "Workshop");
            }

            BossRoomCount = (int)(1 + CurrentFloor / 1.7);
            for (int i = 0; i < BossRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllBossRooms.Items, "Boss");
            }
        }
        else
        {
            PlaceOneRoomFromArray(AllSuperBossRooms.Items, "SuperBoss");
        }

        PlaceDoors(); 
        Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void Save()
    {
        CurrentFloor++;
        PlayerPrefs.SetInt("CurrentFloor", CurrentFloor);
    }

    private void PlaceOneRoomFromArray(GameObject[] PossibleRooms, string type)
    {
        Vector2Int NewRoom = VacantPlacesFind();
        bool NeedDoorUp = false;
        bool NeedDoorDown = false;
        bool NeedDoorLeft = false;
        bool NeedDoorRight = false;

        if(RoomsOnTheMap[NewRoom.x + 1, NewRoom.y])
        {
            NeedDoorRight = true;
        }
        if (RoomsOnTheMap[NewRoom.x - 1, NewRoom.y])
        {
            NeedDoorLeft = true;
        }
        if (RoomsOnTheMap[NewRoom.x, NewRoom.y + 1])
        {
            NeedDoorUp = true;
        }
        if (RoomsOnTheMap[NewRoom.x, NewRoom.y - 1])
        {
            NeedDoorDown = true;
        }

        int i = 0;
        while (true && i < 1000)
        {
            GameObject NewRandomRoom = PossibleRooms[UnityEngine.Random.Range(0, PossibleRooms.Length)];
            if (!(NeedDoorUp && !NewRandomRoom.transform.GetChild(0).GetComponent<Room>().PossibleDoorUp) &&
                !(NeedDoorDown && !NewRandomRoom.transform.GetChild(0).GetComponent<Room>().PossibleDoorDown) &&
                !(NeedDoorLeft && !NewRandomRoom.transform.GetChild(0).GetComponent<Room>().PossibleDoorLeft) &&
                !(NeedDoorRight && !NewRandomRoom.transform.GetChild(0).GetComponent<Room>().PossibleDoorRight))
            {
                RoomsMap[NewRoom.x, NewRoom.y] = Instantiate(NewRandomRoom, new Vector3((NewRoom.x - 12) * 24, (NewRoom.y - 12) * 14, 0), Quaternion.identity);
                RoomsOnTheMap[NewRoom.x, NewRoom.y] = true;
                RoomTypeOnTheMap[NewRoom.x, NewRoom.y] = type;
                break;
            }

            i++;
        }
    }

    private void PlaceDoors()
    {
        for (int x = 0; x < RoomsOnTheMap.GetLength(0); x++)
        {
            for (int y = 0; y < RoomsOnTheMap.GetLength(0); y++)
            {
                if (RoomsOnTheMap[x, y])
                {
                    if (RoomsOnTheMap[x + 1, y])
                    {
                        RoomsMap[x, y].transform.GetChild(0).GetComponent<Room>().DoorRight.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                        RoomsMap[x + 1, y].transform.GetChild(0).GetComponent<Room>().DoorLeft.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                    }

                    if (RoomsOnTheMap[x - 1, y])
                    {
                        RoomsMap[x, y].transform.GetChild(0).GetComponent<Room>().DoorLeft.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                        RoomsMap[x - 1, y].transform.GetChild(0).GetComponent<Room>().DoorRight.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                    }

                    if (RoomsOnTheMap[x, y + 1])
                    {
                        RoomsMap[x, y].transform.GetChild(0).GetComponent<Room>().DoorUp.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                        RoomsMap[x, y + 1].transform.GetChild(0).GetComponent<Room>().DoorDown.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                    }

                    if (RoomsOnTheMap[x, y - 1])
                    {
                        RoomsMap[x, y].transform.GetChild(0).GetComponent<Room>().DoorDown.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                        RoomsMap[x, y - 1].transform.GetChild(0).GetComponent<Room>().DoorUp.GetComponent<ArrayHolder>().Items[1].SetActive(false);
                    }
                }
            }
        }
    }


    private Vector2Int VacantPlacesFind()
    {
        HashSet<Vector2Int> VacantPlaces = new HashSet<Vector2Int>();
        int MaxX = RoomsOnTheMap.GetLength(0);
        int MaxY = RoomsOnTheMap.GetLength(1);

        for (int x = 0; x < MaxX; x++)
        {
            for (int y = 0; y < MaxY; y++)
            {
                if (RoomsOnTheMap[x, y] == false)
                {
                    continue;
                }

                if (x > 1 && RoomsOnTheMap[x - 1, y] == false && IsRoomCanPlaces(x - 1, y) && !RoomsOnTheMap[x - 1, y + 1] && !RoomsOnTheMap[x - 1, y - 1])
                {
                    VacantPlaces.Add(new Vector2Int(x - 1, y));
                }
                if (y > 1 && RoomsOnTheMap[x, y - 1] == false && IsRoomCanPlaces(x, y - 1) && !RoomsOnTheMap[x + 1, y - 1] && !RoomsOnTheMap[x - 1, y - 1])
                {
                    VacantPlaces.Add(new Vector2Int(x, y - 1));
                }
                if (x < MaxX - 1 && RoomsOnTheMap[x + 1, y] == false && IsRoomCanPlaces(x + 1, y) && !RoomsOnTheMap[x + 1, y + 1] && !RoomsOnTheMap[x + 1, y - 1])
                {
                    VacantPlaces.Add(new Vector2Int(x + 1, y));
                }
                if (y < MaxY - 1 && RoomsOnTheMap[x, y + 1] == false && IsRoomCanPlaces(x, y + 1) && !RoomsOnTheMap[x + 1, y + 1] && !RoomsOnTheMap[x - 1, y + 1])
                {
                    VacantPlaces.Add(new Vector2Int(x, y + 1));
                }
            }
        }

        return VacantPlaces.ElementAt(UnityEngine.Random.Range(0, VacantPlaces.Count));
    }


    private bool IsRoomCanPlaces(int x, int y)
    {
        bool Can = true;

        if(RoomsOnTheMap[x + 1, y])
        {
            if (!RoomsMap[x + 1, y].transform.GetChild(0).GetComponent<Room>().PossibleDoorLeft)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x - 1, y])
        {
            if (!RoomsMap[x - 1, y].transform.GetChild(0).GetComponent<Room>().PossibleDoorRight)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x, y + 1])
        {
            if (!RoomsMap[x, y + 1].transform.GetChild(0).GetComponent<Room>().PossibleDoorDown)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x, y - 1])
        {
            if (!RoomsMap[x, y - 1].transform.GetChild(0).GetComponent<Room>().PossibleDoorUp)
            {
                Can = false;
            }
        }

        return Can;
    }

}
