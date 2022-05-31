using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт управления ввода игрока с клавиатуры для персонажа, и не только
/// </summary>
public class KeyboardController : MonoBehaviour
{
    private Player _Player;
    private OpenMap _Map;
    private BlackBG _BlackBG;
    private Transform _Canvas;
    private Save _Save;

    private void Start()
    {
        _Player = GetComponent<Player>();
        _Map = GameObject.Find("Map").GetComponent<OpenMap>();
        _BlackBG = GameObject.Find("BlackBG").GetComponent<BlackBG>();
        _Canvas = GameObject.Find("Main Camera/Canvas").GetComponent<Transform>();
        _Save = GameObject.Find("FloorSpawner").GetComponent<Save>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (_Canvas.localScale.z != 1)
            {
                _Canvas.localScale = new Vector3(0.1f, 0.1f, 1);
            }
            else
            {
                _Canvas.localScale = new Vector3(0, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _Player.Dash();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _Map.TogleMap();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(CloseGame());
        }
    }

    private IEnumerator CloseGame()
    {
        yield return StartCoroutine(_BlackBG.ToDark());
        _Save.SaveGame(false);
        SceneManager.LoadScene("Main menu");
    }
}
