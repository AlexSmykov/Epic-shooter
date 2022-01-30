using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{

    public Vector3 CameraChangePosition;
    public Vector3 PlayerChangePosition;
    private Animator animator;
    private Camera Cam;

    private void Start()
    {
        Cam = Camera.main.GetComponent<Camera>();
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(Vector3 CameraNewPosition, Vector3 PlayerNewPosition)
    {
        CameraChangePosition = CameraNewPosition;
        PlayerChangePosition = PlayerNewPosition;
        animator.Play("RoomChangeForCamera");
    }

    public void CameraMover()
    {
        Cam.transform.position += CameraChangePosition;
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(Cam.transform.position.x + PlayerChangePosition.x, Cam.transform.position.y + PlayerChangePosition.y, 0);
        foreach (GameObject trash in GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<FloorMaker>().Trash)
        {
            Destroy(trash);
        }
        GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<FloorMaker>().Trash = new List<GameObject>();
    }

    public void CloseGame()
    {
        SceneManager.LoadScene("Main menu");
    }

    public void CompleteGame()
    {
        SceneManager.LoadScene("Congratulations");
    }
}
