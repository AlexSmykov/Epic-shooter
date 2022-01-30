using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public void SaveGame(Collider2D collision)
    {
        GetComponent<FloorMaker>().Save();
        collision.GetComponent<Player>().Save();
        collision.GetComponentInChildren<WeaponSwitch>().Save();
        foreach (Transform Weapon in GameObject.FindGameObjectWithTag("PlaceForGun").GetComponent<Transform>())
        {
            Weapon.GetComponent<Gun>().Save();
        }
    }
}
