using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для сохранения состояний игрока, карты и ресурсов. Сохраняется при переходе на новый этаж, зачистке комнаты и выходе в меню
/// </summary>
public class Save : MonoBehaviour
{
    private Saver _Saver;
    private Player _Player;
    private FloorMaker _FloorMaker;
    private OpenMap _Map;
    private bool _SaveAllowed;
    public bool SaveAllowed { get { return _SaveAllowed; } set { _SaveAllowed = value; } }

    private void Start()
    {
        _SaveAllowed = true;
        _Saver = FindObjectOfType<Saver>();
        _Player = FindObjectOfType<Player>();
        _FloorMaker = GetComponent<FloorMaker>();
        _Map = GameObject.Find("Map").GetComponent<OpenMap>();
    }

    public void SaveGame(bool IsNewFloor)
    {
        if (_SaveAllowed)
        {
            _Saver.Save();
            _Player.GetComponent<Player>().Save();
            _Player.GetComponentInChildren<WeaponSwitch>().Save();

            if (IsNewFloor)
            {
                _FloorMaker.NewFloor();
                _Map.ResetSave();
                _Player.GetComponent<Player>().ResetSavePosition();
            }

        }
    }
}
