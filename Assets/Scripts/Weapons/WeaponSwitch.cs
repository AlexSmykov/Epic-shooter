using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Скрипт для управления оружиями
/// </summary>
public class WeaponSwitch : MonoBehaviour
{
    private int WeaponIndex = 0;
    private int CurrentWeaponCount;
    [SerializeField] private int WeaponCount;
    [SerializeField] private bool[] UnlockedWeapons;

    void Start()
    {
        SelectWeapon();
    }

    public void Save()
    {
        for (int i = 0; i < UnlockedWeapons.Length; i++)
        {
            transform.GetChild(i).GetComponent<Weapon>().Save();
            PlayerPrefs.SetString("Weapon" + i.ToString() + "Unlocked", UnlockedWeapons[i].ToString());
        }

        PlayerPrefs.SetInt("WeaponIndex", WeaponIndex);
    }

    public void Load()
    {
        WeaponCount = transform.childCount;
        UnlockedWeapons = new bool[WeaponCount];
        WeaponIndex = PlayerPrefs.GetInt("WeaponIndex", WeaponIndex);

        for (int i = 0; i < UnlockedWeapons.Length; i++)
        {
            UnlockedWeapons[i] = bool.Parse(PlayerPrefs.GetString("Weapon" + i.ToString() + "Unlocked", "false"));

            Weapon weapon = transform.GetChild(i).GetComponent<Weapon>();
            weapon.Init();
            weapon.Load();
        }
    }

    void Update()
    {
        //Оружие можно менять на кнопки с цифрами иил колёсико мышки
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            while(true)
            {
                WeaponIndex++;
                WeaponIndex %= WeaponCount;

                if(UnlockedWeapons[WeaponIndex])
                {
                    break;
                }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            while(true)
            {
                WeaponIndex--;

                if (WeaponIndex == -1)
                {
                    WeaponIndex = WeaponCount - 1;
                }

                if (UnlockedWeapons[WeaponIndex])
                {
                    break;
                }
            }
        }

        for (int i = 0; i < WeaponCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()) && UnlockedWeapons[i] == true)
            {
                WeaponIndex = i;
            }
        }

        if(CurrentWeaponCount != WeaponIndex)
        {
            SelectWeapon();
        }
    }

    /// <summary>
    /// Активировать оружие с новым индексом
    /// </summary>
    public void SelectWeapon()
    {
        int i = 0;

        foreach(Transform weapon in transform)
        {
            if(i == WeaponIndex)
            {
                weapon.gameObject.SetActive(true);
                weapon.gameObject.GetComponent<Weapon>().BulletTextUpdate();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }

        CurrentWeaponCount = WeaponIndex;
    }

    /// <summary>
    /// Разблочить оружие по переданному типу
    /// </summary>
    public void UnlockWeapon(StateMachine.WeaponLink Weapon)
    {
        UnlockedWeapons[(int)Weapon - 1] = true;
    }
}
