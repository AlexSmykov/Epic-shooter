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
    private GameObject Player;
    private GameObject FloorSpawner;

    private void Start()
    {
        Cam = Camera.main.GetComponent<Camera>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        FloorSpawner = GameObject.FindGameObjectWithTag("FloorSpawner");
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
        if (CameraChangePosition.x > 0)
        {
            Player.GetComponent<Player>().Cords = new Vector2Int(Player.GetComponent<Player>().Cords.x + 1, Player.GetComponent<Player>().Cords.y);
        }
        else if(CameraChangePosition.x < 0)
        {
            Player.GetComponent<Player>().Cords = new Vector2Int(Player.GetComponent<Player>().Cords.x - 1, Player.GetComponent<Player>().Cords.y);
        }
        else if (CameraChangePosition.y > 0)
        {
            Player.GetComponent<Player>().Cords = new Vector2Int(Player.GetComponent<Player>().Cords.x, Player.GetComponent<Player>().Cords.y + 1);
        }
        else if (CameraChangePosition.y < 0)
        {
            Player.GetComponent<Player>().Cords = new Vector2Int(Player.GetComponent<Player>().Cords.x, Player.GetComponent<Player>().Cords.y - 1);
        }
        Player.transform.position = new Vector3(Cam.transform.position.x + PlayerChangePosition.x, Cam.transform.position.y + PlayerChangePosition.y, 0);
        FloorSpawner.GetComponent<FloorMaker>().RoomsChecked[Player.GetComponent<Player>().Cords.x, Player.GetComponent<Player>().Cords.y] = true;


        if (Player.GetComponent<OpenMap>().Map.activeSelf)
        {
            Player.GetComponent<OpenMap>().UpdateMap();
        }
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
