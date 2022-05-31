using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт, который срабатывает при захождении игрока в дверь, для перемещения в след. комнату
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class DoorTrigger : MonoBehaviour
{
    private RoomChanger _Changer;
    private bool _IsEntering;

    public enum DoorWay
    {
        Left,
        Right,
        Top,
        Bottom,
    }

    [SerializeField] private DoorWay _state;

    private void Start()
    {
        _Changer = GameObject.Find("Main Camera").GetComponent<RoomChanger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_IsEntering)
            {
                StartCoroutine(_Changer.Change(_state));
                _IsEntering = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _IsEntering = false;
        }
    }
}
