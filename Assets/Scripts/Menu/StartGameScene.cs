using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ ��� ������ ���� �� ����
/// </summary>
public class StartGameScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Alex's scene");
    }
}
