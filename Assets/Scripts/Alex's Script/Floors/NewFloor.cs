using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewFloor : MonoBehaviour
{
    private float SpawnTime = 2;
    private bool FloorChanging = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !FloorChanging && SpawnTime <= 0)
        {
            FloorChanging = true;
            GameObject Bg = GameObject.FindGameObjectWithTag("BlackBG");
            Bg.SetActive(true);
            Bg.GetComponent<Animator>().Play("BgFloorEnd");
            StartCoroutine(EndFloor(collision));
        }
    }

    private void Update()
    {
        if(SpawnTime > 0)
        {
            SpawnTime -= Time.deltaTime;   
        }
    }

    private IEnumerator EndFloor(Collider2D collision)
    {
        GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<Save>().SaveGame(collision);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Alex's scene");
        yield return new WaitForSeconds(2);
    }
}
