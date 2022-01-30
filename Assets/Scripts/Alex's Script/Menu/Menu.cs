using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Bg;
    public GameObject MenuObject;

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        Bg.GetComponent<Animator>().Play("MenuEnds");
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

    public void ContinueGame()
    {
        Bg.GetComponent<Animator>().Play("MenuEnds");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
