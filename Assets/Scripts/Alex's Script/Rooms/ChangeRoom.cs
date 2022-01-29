using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public Vector3 CameraChangePosition;
    public Vector3 PlayerChangePosition;
    private GameObject BlackBG;

    void Start()
    {
        BlackBG = GameObject.FindGameObjectWithTag("BlackBG");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BlackBG.GetComponent<CameraMove>().PlayAnimation(CameraChangePosition, PlayerChangePosition);
        }
    }
}
