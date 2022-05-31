using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для смены комнаты
/// </summary>
public class RoomChanger : MonoBehaviour
{
    private Vector2Int _RoomSize = new Vector2Int(24, 14);
    private BlackBG _BG;
    private Transform _Camera, _Player;
    private OpenMap _Map;

    void Start()
    {
        _BG = transform.Find("BlackBG").GetComponent<BlackBG>();
        _Camera = GetComponent<Camera>().transform;
        _Map = GameObject.Find("Map").GetComponent<OpenMap>();
    }

    public void PlayerConnect(Transform player)
    {
        _Player = player;
    }

    public IEnumerator Change(DoorTrigger.DoorWay state)
    {
        yield return StartCoroutine(_BG.ToDark());

        _Map.MovePlayer((OpenMap.Ways) System.Enum.Parse(typeof(OpenMap.Ways), state.ToString()));

        if (state == DoorTrigger.DoorWay.Left)
        {
            _Camera.position = new Vector3(_Camera.position.x - _RoomSize.x, _Camera.position.y, _Camera.position.z);
            _Player.position = new Vector3(_Camera.position.x + _RoomSize.x * 0.3f, _Camera.position.y, _Camera.position.z);
        }
        else if (state == DoorTrigger.DoorWay.Right)
        {
            _Camera.position = new Vector3(_Camera.position.x + _RoomSize.x, _Camera.position.y, _Camera.position.z);
            _Player.position = new Vector3(_Camera.position.x - _RoomSize.x * 0.3f, _Camera.position.y, _Camera.position.z);
        }
        else if (state == DoorTrigger.DoorWay.Bottom)
        {
            _Camera.position = new Vector3(_Camera.position.x, _Camera.position.y - _RoomSize.y, _Camera.position.z);
            _Player.position = new Vector3(_Camera.position.x, _Camera.position.y + _RoomSize.y * 0.25f, _Camera.position.z);
        }
        else if (state == DoorTrigger.DoorWay.Top)
        {
            _Camera.position = new Vector3(_Camera.position.x, _Camera.position.y + _RoomSize.y, _Camera.position.z);
            _Player.position = new Vector3(_Camera.position.x, _Camera.position.y - _RoomSize.y * 0.22f, _Camera.position.z);
        }

        yield return StartCoroutine(_BG.ToLight());
    }
}
