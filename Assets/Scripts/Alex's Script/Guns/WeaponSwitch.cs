using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int WeaponIndex = 0;
    private int CurrentWeaponCount;
    public int WeaponCount;
    public bool[] UnlockedGuns = new bool[6];
    public bool AllWeaponsHave;

    void Start()
    {
        for(int i = 0; i < UnlockedGuns.Length; i++)
        {
            string Active = PlayerPrefs.GetString("Weapon" + i.ToString() + "Unlocked", "false");
            if(Active == "false")
            {
                UnlockedGuns[i] = false;
            }
            else
            {
                UnlockedGuns[i] = true;
            }
        }
        UnlockedGuns[GetComponentInParent<Player>().StartWeaponIndex] = true;
        SelectWeapon();
    }

    public void Save()
    {
        for (int i = 0; i < UnlockedGuns.Length; i++)
        {
            string Active;
            if (UnlockedGuns[i])
            {
                Active = "true";
            }
            else
            {
                Active = "false";
            }
            PlayerPrefs.SetString("Weapon" + i.ToString() + "Unlocked", Active);
        }
    }

    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            while(true)
            {
                WeaponIndex++;
                WeaponIndex %= WeaponCount;

                if(UnlockedGuns[WeaponIndex])
                {
                    break;
                }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            while(true)
            {
                WeaponIndex--;
                if (WeaponIndex == -1)
                {
                    WeaponIndex = WeaponCount - 1;
                }

                if (UnlockedGuns[WeaponIndex])
                {
                    break;
                }
            }
        }

        if (Input.GetKeyDown("1") && UnlockedGuns[0] == true)
        {
            WeaponIndex = 0;
        }
        else if (Input.GetKeyDown("2") && UnlockedGuns[1] == true)
        {
            WeaponIndex = 1;
        }
        else if (Input.GetKeyDown("3") && UnlockedGuns[2] == true)
        {
            WeaponIndex = 2;
        }
        else if (Input.GetKeyDown("4") && UnlockedGuns[3] == true)
        {
            WeaponIndex = 3;
        }
        else if (Input.GetKeyDown("5") && UnlockedGuns[4] == true)
        {
            WeaponIndex = 4;
        }
        else if (Input.GetKeyDown("6") && UnlockedGuns[5] == true)
        {
            WeaponIndex = 5;
        }

        if(CurrentWeaponCount != WeaponIndex)
        {
            SelectWeapon();
        }

        WeaponsHaveCheck();
    }

    public void SelectWeapon()
    {
        int i = 0;

        foreach(Transform Weapon in transform)
        {
            if(i == WeaponIndex)
            {
                Weapon.gameObject.SetActive(true);
                Weapon.gameObject.GetComponent<Gun>().BulletTextUpdate();
            }
            else
            {
                Weapon.gameObject.SetActive(false);
            }

            i++;
        }

        CurrentWeaponCount = WeaponIndex;
    }

    private void WeaponsHaveCheck()
    {
        AllWeaponsHave = true;
        foreach (bool Weapon in UnlockedGuns)
        {
            if(!Weapon)
            {
                AllWeaponsHave = false;
            }
        }
    }
}
