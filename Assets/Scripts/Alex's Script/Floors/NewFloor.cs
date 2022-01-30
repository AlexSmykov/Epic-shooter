using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject Bg = GameObject.FindGameObjectWithTag("BlackBG");
            Bg.SetActive(true);
            Bg.GetComponent<Animator>().Play("BgFloorEnd");
            StartCoroutine(EndFloor(collision));
        }
    }

    private IEnumerator EndFloor(Collider2D collision)
    {
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("FloorSpawner").GetComponent<FloorMaker>().Save();
        collision.GetComponent<Player>().Save();
        collision.GetComponentInChildren<WeaponSwitch>().Save();
        foreach (Transform Weapon in GameObject.FindGameObjectWithTag("PlaceForGun").GetComponent<Transform>())
        {
            Weapon.GetComponent<Gun>().Save();
        }
        SceneManager.LoadScene("Alex's scene");
        yield return new WaitForSeconds(2);
    }
}
