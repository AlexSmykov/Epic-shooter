using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для главного меню (когда включается игра)
/// </summary>
public class Menu : MonoBehaviour
{
    private BlackBG _Bg;
    private StartGameScene _StartGameScene;
    public GameObject MenuObject;

    private void Start()
    {
        _Bg = GameObject.Find("BlackBg").GetComponent<BlackBG>();
        _StartGameScene = GameObject.Find("BlackBg").GetComponent<StartGameScene>();

        _Bg.Init();
        StartCoroutine(_Bg.ToLight());
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("NewGameStatus", "HaveSave");
        StartCoroutine(StartGame());
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("NewGameStatus"))
        {
            StartCoroutine(StartGame());
        }
        else
        {
            NewGame();
        }
    }

    private IEnumerator StartGame()
    {
        yield return StartCoroutine(_Bg.ToDark());
        _StartGameScene.StartGame();
    }

    public void TutorialOpen()
    {
        PlayerPrefs.DeleteAll();
        MenuObject.GetComponent<Animator>().Play("TutorialOpen");
    }
    public void TutorialClose()
    {
        PlayerPrefs.DeleteAll();
        MenuObject.GetComponent<Animator>().Play("TutorialClose");
    }

    public void Exit()
    {
        StartCoroutine(CloseGame());
    }

    private IEnumerator CloseGame()
    {
        yield return StartCoroutine(_Bg.ToDark());
        Application.Quit();
    }
}
