using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт для начала игры из меню
/// </summary>
public class StartGameScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Alex's scene");
    }
}
