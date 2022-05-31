using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт для создания нового этажа (ну или подземелья), активируется при касании лестницы
/// </summary>
public class NewFloor : MonoBehaviour
{
    private float _SpawnTime = 2;
    private bool _FloorChanging = false;
    private BlackBG _BlackBG;

    private void Start()
    {
        _BlackBG = GameObject.Find("BlackBG").GetComponent<BlackBG>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_FloorChanging && _SpawnTime <= 0)
        {
            _FloorChanging = true;
            GameObject Bg = GameObject.FindGameObjectWithTag("BlackBG");
            Bg.SetActive(true);
            Bg.GetComponent<Animator>().Play("BgFloorEnd");
            StartCoroutine(EndFloor(collision));
        }
    }

    private void Update()
    {
        if(_SpawnTime > 0)
        {
            _SpawnTime -= Time.deltaTime;   
        }
    }

    private IEnumerator EndFloor(Collider2D collision)
    {
        GameObject.Find("FloorSpawner").GetComponent<Save>().SaveGame(true);
        yield return StartCoroutine(_BlackBG.ToDark());
        SceneManager.LoadScene("Alex's scene");
        yield return new WaitForSeconds(1);
    }
}
