using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// —крипт дл€ карты, показывает важные места и те, где вы уже побывали
/// </summary>
public class OpenMap : MonoBehaviour
{
    public enum Ways
    {
        Left,
        Right,
        Top,
        Bottom
    }

    private Transform _Map;
    private GameObject _FloorSpawner;
    private Camera _Camera;

    [SerializeField] private GameObject _Tile;
    [SerializeField] private Vector2Int _MapSize;
    public Vector2Int MapSize { get { return _MapSize; } }

    private bool[,] _RoomsChecked;
    private GameObject[,] _Tiles;
    private Vector2Int _CurrentCords = new Vector2Int(0, 0);
    public Vector2Int CurrentCords { get { return _CurrentCords; } }

    private Color UNEXPLORED =  new Color(140f / 255f, 140f / 255f, 140f / 255f, 180f / 255f);
    private Color EXPLORED = new Color(200f / 255f, 200f / 255f, 200f / 255f, 200f / 255f);
    private Color CURRENT = new Color(255f / 255f, 255f / 255f, 255f / 255f, 220f / 255f);

    public void Init()
    {
        _Camera = Camera.main;
        _RoomsChecked = new bool[_MapSize.x, _MapSize.y];
        _Map = GameObject.Find("Map").transform;
        _FloorSpawner = GameObject.Find("FloorSpawner");
        _Tiles = new GameObject[_MapSize.x, _MapSize.y];
        Load();
    }

    /// <summary>
    /// ¬кл/выкл карту
    /// </summary>
    public void TogleMap()
    {
        if (_Map.gameObject.activeSelf)
        {
            _Map.gameObject.SetActive(false);
            return;
        }

        _Map.gameObject.SetActive(true);
        UpdateMap();
    }

    /// <summary>
    /// ƒл€ построенни€ карты (единожды сначала)
    /// </summary>
    public IEnumerator CreateMap()
    {
        for (int i = 0; i < _MapSize.x; i++)
        {
            for (int j = 0; j < _MapSize.y; j++)
            {
                if (_FloorSpawner.GetComponent<FloorMaker>().RoomsOnTheMap[i, j])
                {
                    Vector3 TilePosition = new Vector3(i * 0.45f + 3.3f + _Camera.gameObject.transform.position.x, 
                        j * 0.45f - 1.5f + _Camera.gameObject.transform.position.y, 
                        10);
                    GameObject tile = Instantiate(_Tile, TilePosition, Quaternion.identity, _Map);
                    DelOtherTileImages(tile.transform, _FloorSpawner.GetComponent<FloorMaker>().RoomTypeOnTheMap[i, j]);
                    _Tiles[i, j] = tile;
                }
            }
        }

        yield return new WaitForEndOfFrame();
        UpdateMap();
    }

    public void UpdateMap()
    {
        for (int i = 0; i < _MapSize.x; i++)
        {
            for (int j = 0; j < _MapSize.y; j++)
            {
                if (_Tiles[i, j])
                {
                    GameObject tile = _Tiles[i, j];
                    tile.GetComponent<SpriteRenderer>().color = UNEXPLORED;

                    if ((_CurrentCords.x == i && _CurrentCords.y == j) || _RoomsChecked[i, j])
                    {
                        if (tile.transform.childCount != 0)
                        {
                            tile.transform.GetChild(0).gameObject.SetActive(true);
                        }

                        if (_RoomsChecked[i, j])
                        {
                            tile.GetComponent<SpriteRenderer>().color = EXPLORED;
                        }

                        if (_CurrentCords.x == i && _CurrentCords.y == j)
                        {
                            _RoomsChecked[i, j] = true;
                            tile.GetComponent<SpriteRenderer>().color = CURRENT;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// »значально, на €чейке карты много разных значков, удал€ютс€ все, кроме переданного
    /// </summary>
    private void DelOtherTileImages(Transform tile, Factory.Rooms type)
    {
        for(int i = 0; i < tile.childCount; i++)
        {
            if (tile.GetChild(i).gameObject.name != type.ToString())
            {
                Destroy(tile.GetChild(i).gameObject);
            }
            else
            {
                tile.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// —рабатывает при перемещении игрока между комнатами
    /// </summary>
    public void MovePlayer(Ways way)
    {
        if(way == Ways.Top)
        {
            _CurrentCords.y += 1;
        }

        else if (way == Ways.Bottom)
        {
            _CurrentCords.y -= 1;
        }

        else if (way == Ways.Right)
        {
            _CurrentCords.x += 1;
        }

        else if (way == Ways.Left)
        {
            _CurrentCords.x -= 1;
        }

        Save();
        UpdateMap();
    }

    public void CheckRoom(Room room)
    {
        if (room.Cleared)
        {
            _RoomsChecked[(int)room.transform.position.x / 24 + _MapSize.x / 2, (int)room.transform.position.y / 14 + _MapSize.y / 2] = true;
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.SetString("CurrentCords", new Vector2Int(_MapSize.x / 2, _MapSize.y / 2).ToString());
    }

    public void Save()
    {
        PlayerPrefs.SetString("CurrentCords", _CurrentCords.ToString());
    }

    public void Load()
    {
        string cords = PlayerPrefs.GetString("CurrentCords", new Vector2Int(_MapSize.x / 2, _MapSize.y / 2).ToString());
        string[] cords_separate = cords.Substring(1, cords.Length - 2).Split(',');
        _CurrentCords = new Vector2Int(int.Parse(cords_separate[0]), int.Parse(cords_separate[1]));
        _Camera.transform.position = new Vector3((CurrentCords.x - MapSize.x / 2) * 24, (CurrentCords.y - MapSize.y / 2) * 14, -10);
    }
}
