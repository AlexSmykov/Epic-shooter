using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Скрипт для сохранения заспавненных комнат и предметов
/// </summary>
[Serializable]
public class Saver : MonoBehaviour
{
    [SerializeField] private List<HashSet<GameObject>> _ItemsToSave = new List<HashSet<GameObject>>();

    private List<string> _SavePaths = new List<string>();
    private Factory _Factory = new Factory();

    const string ROOM_SAVE_NAME = "Resources/RoomSave.json";
    const string OBJECTS_SAVE_NAME = "Resources/ObjectSave.json";
    const string SHOP_PLACES_SAVE_NAME = "Resources/ShopPlaceSave.json";

    private void Awake()
    {
        _SavePaths.Add(Path.Combine(Application.dataPath, ROOM_SAVE_NAME));
        _SavePaths.Add(Path.Combine(Application.dataPath, OBJECTS_SAVE_NAME));
        _SavePaths.Add(Path.Combine(Application.dataPath, SHOP_PLACES_SAVE_NAME));

        for (int i = 0; i < _SavePaths.Count; i++)
        {
            _ItemsToSave.Add(new HashSet<GameObject>());
        }
    }

    public void AddRoom(GameObject room)
    {
        _ItemsToSave[0].Add(room);
    }
    public void DeleteRoom(GameObject room)
    {
        _ItemsToSave[0].Remove(room);
    }

    public void AddObject(GameObject obj)
    {
        _ItemsToSave[1].Add(obj);
    }
    public void DeleteObject(GameObject obj)
    {
        _ItemsToSave[1].Remove(obj);
    }

    public void AddShopPlace(GameObject obj)
    {
        _ItemsToSave[2].Add(obj);
    }
    public void DeleteShopPlace(GameObject obj)
    {
        _ItemsToSave[2].Remove(obj);
    }

    public bool SaveCheck()
    {
        bool state = true;
        foreach(string path in _SavePaths)
        {
            if (!File.Exists(path))
            {
                state = false;
            }
        }

        return state;
    }

    public void Save()
    {
        File.WriteAllText(_SavePaths[0], "");
        File.WriteAllText(_SavePaths[1], "");
        File.WriteAllText(_SavePaths[2], "");

        foreach (GameObject room in _ItemsToSave[0])
        {
            File.AppendAllText(_SavePaths[0], JsonUtility.ToJson(
                new RoomParameters(room.transform.position, room.name.Split('(')[0], room.GetComponent<Room>().RoomType, room.GetComponent<Room>().Cleared)));
            File.AppendAllText(_SavePaths[0], "\n");
        }

        foreach (GameObject obj in _ItemsToSave[1])
        {
            File.AppendAllText(_SavePaths[1], JsonUtility.ToJson(
                new ObjectsParameters(obj.transform.position, obj.name.Split('(')[0])));
            File.AppendAllText(_SavePaths[1], "\n");
        }

        foreach (GameObject obj in _ItemsToSave[2])
        {
            File.AppendAllText(_SavePaths[2], JsonUtility.ToJson(
                new ShopPlaceParameters(obj.transform.position, obj.name.Split('(')[0], obj.GetComponent<ShopPlace>().ItemName)));
            File.AppendAllText(_SavePaths[2], "\n");
        }

        PlayerPrefs.SetString("IsNewGame", "false");
    }

    public List<GameObject> Load()
    {
        string[] JsonFileRoom = File.ReadAllLines(_SavePaths[0]);
        List<RoomParameters> RoomsJson = new List<RoomParameters>();
        foreach (string json in JsonFileRoom)
        {
            RoomsJson.Add(JsonUtility.FromJson<RoomParameters>(json));
        }

        string[] JsonFileObject = File.ReadAllLines(_SavePaths[1]);
        List<ObjectsParameters> ObjectsJson = new List<ObjectsParameters>();
        foreach (string json in JsonFileObject)
        {
            ObjectsJson.Add(JsonUtility.FromJson<ObjectsParameters>(json));
        }

        string[] JsonFileShopPlace = File.ReadAllLines(_SavePaths[2]);
        List<ShopPlaceParameters> ShopPlacesJson = new List<ShopPlaceParameters>();
        foreach (string json in JsonFileShopPlace)
        {
            ShopPlacesJson.Add(JsonUtility.FromJson<ShopPlaceParameters>(json));
        }

        foreach (ObjectsParameters item in ObjectsJson)
        {
            AddObject(Instantiate(_Factory.CreateDeterminedSaveObjectByName(item.Name), item.Position, Quaternion.identity));
        }

        foreach (ShopPlaceParameters item in ShopPlacesJson)
        {
            GameObject NewRoom = Instantiate(_Factory.CreateDeterminedSaveObjectByName(item.Name), item.Position, Quaternion.identity);
            NewRoom.GetComponent<ShopPlace>().OnLoad(_Factory.CreateDeterminedSaveObjectByName(item.ItemName));
            NewRoom.GetComponent<ShopPlace>().CreateItem();
            AddShopPlace(NewRoom);
        }

        List<GameObject> Rooms = new List<GameObject>();
        foreach (RoomParameters item in RoomsJson)
        {
            GameObject NewRoom = _Factory.CreateDeterminedRoom(item.Type, item.Name, item.Cleared);
            NewRoom.transform.position = item.Position;

            NewRoom.transform.rotation = Quaternion.identity;
            Rooms.Add(NewRoom);
        }

        File.Delete(_SavePaths[0]);
        File.Delete(_SavePaths[1]);
        File.Delete(_SavePaths[2]);

        return Rooms;
    }
}

[Serializable]
public class ObjectToSave
{
    public Vector3 Position;
    public string Name;

    public ObjectToSave() { }
}

[Serializable]
public class RoomParameters : ObjectToSave
{
    public Factory.Rooms Type;
    public bool Cleared;

    public RoomParameters(Vector3 Position, string Name, Factory.Rooms Type, bool Cleared)
    {
        this.Position = Position; 
        this.Name = Name;
        this.Type = Type;
        this.Cleared = Cleared;
    }
}


[Serializable]
public class ObjectsParameters : ObjectToSave
{
    public ObjectsParameters(Vector3 Position, string Name)
    {
        this.Position = Position;
        this.Name = Name;
    }
}

[Serializable]
public class ShopPlaceParameters : ObjectToSave
{
    public string ItemName;

    public ShopPlaceParameters(Vector3 Position, string Name, string ItemName)
    {
        this.Position = Position;
        this.Name = Name;
        this.ItemName = ItemName;
    }
}
