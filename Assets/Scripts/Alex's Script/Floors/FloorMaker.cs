using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class FloorMaker : MonoBehaviour
{
    public bool[,] RoomsOnTheMap;
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
    public List<GameObject> Trash;

    public int DefaultRoomCount;
    public int ShopRoomCount;
    public int WorkshopRoomCount;
    public int ChestRoomCount;
    public int BossRoomCount;
    public int CurrentFloor;
    public int MaxFloor;

    private void Start()
    {
        RoomsOnTheMap = new bool[50, 50];
        RoomsMap = new GameObject[50, 50];
        RoomsOnTheMap[25, 25] = true;
        RoomsMap[25, 25] = Instantiate(AllStartRooms.Items[UnityEngine.Random.Range(0, AllStartRooms.Items.Length)], new Vector3(0, 0, 0), Quaternion.identity); 
        Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(Storage, new Vector3(0, 0, 0), Quaternion.identity);

        CurrentFloor = PlayerPrefs.GetInt("CurrentFloor", 1);
        if(CurrentFloor <= MaxFloor)
        {
            GameObject.FindGameObjectWithTag("CurrentFloor").GetComponent<Text>().text = "Current floor: " + CurrentFloor.ToString() + "/" + MaxFloor.ToString();
            DefaultRoomCount = UnityEngine.Random.Range(8 + CurrentFloor, 11 + CurrentFloor * 3);
            for (int i = 0; i < DefaultRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllDefaultRooms.Items);
            }

            ChestRoomCount = UnityEngine.Random.Range(1 + CurrentFloor, (int)(3 + CurrentFloor * 1.5));
            for (int i = 0; i < ChestRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllChestRooms.Items);
            }

            ShopRoomCount = 1 + CurrentFloor / 3;
            for (int i = 0; i < ShopRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllShopRooms.Items);
            }

            WorkshopRoomCount = 1 + CurrentFloor / 3;
            for (int i = 0; i < WorkshopRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllWorkshopRooms.Items);
            }

            BossRoomCount = (int)(1 + CurrentFloor / 1.5);
            for (int i = 0; i < BossRoomCount; i++)
            {
                PlaceOneRoomFromArray(AllBossRooms.Items);
            }
        }
        else
        {
            PlaceOneRoomFromArray(AllSuperBossRooms.Items);
        }

        PlaceDoors();
    }

    public void Save()
    {
        CurrentFloor++;
        PlayerPrefs.SetInt("CurrentFloor", CurrentFloor);
    }

    private void PlaceOneRoomFromArray(GameObject[] PossibleRooms)
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
            if (!(NeedDoorUp && !NewRandomRoom.GetComponent<Room>().PossibleDoorUp) &&
                !(NeedDoorDown && !NewRandomRoom.GetComponent<Room>().PossibleDoorDown) &&
                !(NeedDoorLeft && !NewRandomRoom.GetComponent<Room>().PossibleDoorLeft) &&
                !(NeedDoorRight && !NewRandomRoom.GetComponent<Room>().PossibleDoorRight))
            {
                RoomsMap[NewRoom.x, NewRoom.y] = Instantiate(NewRandomRoom, new Vector3((NewRoom.x - 25) * 24, (NewRoom.y - 25) * 14, 0), Quaternion.identity);
                RoomsOnTheMap[NewRoom.x, NewRoom.y] = true;
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
                        RoomsMap[x, y].GetComponent<Room>().DoorRight.SetActive(true);
                        RoomsMap[x, y].GetComponent<Room>().DoorRightLocker.SetActive(false);
                        RoomsMap[x + 1, y].GetComponent<Room>().DoorLeft.SetActive(true);
                        RoomsMap[x + 1, y].GetComponent<Room>().DoorLeftLocker.SetActive(false);
                    }
                    else
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorRight.SetActive(false);
                        RoomsMap[x, y].GetComponent<Room>().DoorRightLocker.SetActive(true);
                    }

                    if (RoomsOnTheMap[x - 1, y])
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorLeft.SetActive(true);
                        RoomsMap[x, y].GetComponent<Room>().DoorLeftLocker.SetActive(false);
                        RoomsMap[x - 1, y].GetComponent<Room>().DoorRight.SetActive(true);
                        RoomsMap[x - 1, y].GetComponent<Room>().DoorRightLocker.SetActive(false);
                    }
                    else
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorLeft.SetActive(false);
                        RoomsMap[x, y].GetComponent<Room>().DoorLeftLocker.SetActive(true);
                    }

                    if (RoomsOnTheMap[x, y + 1])
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorUp.SetActive(true);
                        RoomsMap[x, y].GetComponent<Room>().DoorUpLocker.SetActive(false);
                        RoomsMap[x, y + 1].GetComponent<Room>().DoorDown.SetActive(true);
                        RoomsMap[x, y + 1].GetComponent<Room>().DoorDownLocker.SetActive(false);
                    }
                    else
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorUp.SetActive(false);
                        RoomsMap[x, y].GetComponent<Room>().DoorUpLocker.SetActive(true);
                    }

                    if (RoomsOnTheMap[x, y - 1])
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorDown.SetActive(true);
                        RoomsMap[x, y].GetComponent<Room>().DoorDownLocker.SetActive(false);
                        RoomsMap[x, y - 1].GetComponent<Room>().DoorUp.SetActive(true);
                        RoomsMap[x, y - 1].GetComponent<Room>().DoorUpLocker.SetActive(false);
                    }
                    else
                    {
                        RoomsMap[x, y].GetComponent<Room>().DoorDown.SetActive(false);
                        RoomsMap[x, y].GetComponent<Room>().DoorDownLocker.SetActive(true);
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
            if (!RoomsMap[x + 1, y].GetComponent<Room>().PossibleDoorLeft)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x - 1, y])
        {
            if (!RoomsMap[x - 1, y].GetComponent<Room>().PossibleDoorRight)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x, y + 1])
        {
            if (!RoomsMap[x, y + 1].GetComponent<Room>().PossibleDoorDown)
            {
                Can = false;
            }
        }
        if (RoomsOnTheMap[x, y - 1])
        {
            if (!RoomsMap[x, y - 1].GetComponent<Room>().PossibleDoorUp)
            {
                Can = false;
            }
        }

        return Can;
    }

}
