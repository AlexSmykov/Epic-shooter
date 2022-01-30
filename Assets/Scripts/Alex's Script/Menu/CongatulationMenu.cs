using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
