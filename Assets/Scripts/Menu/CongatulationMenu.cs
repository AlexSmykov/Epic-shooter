using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//TODO FIX (было уж слишком лень фиксить этот скрипт)

/// <summary>
/// Скрипт для управления меню при окончании игры
/// </summary>
public class CongatulationMenu : MonoBehaviour
{
    public GameObject Bg;
    public void Exit()
    {
        SceneManager.LoadScene("Main menu");
    }

    public void GoToMainMenuButtonPressed()
    {
        Bg.GetComponent<Animator>().Play("CongratsEnds");
    }
}
