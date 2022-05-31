using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс улучшения. Рефакторить публичные переменные в приватные решил не делать, так как сбросятся настроенные параметры для всех префабов, а заполнять заного очень долго
/// </summary>
[RequireComponent(typeof(ItemEffects))]
[RequireComponent(typeof(WithId))]
[RequireComponent(typeof(Pickupable))]
public class Item : MonoBehaviour
{
    public string Name;
    public string Description;
    public GameObject PickupEffect;

    [HideInInspector] public float SpawnTimeReload = 0;

    public int DamageUp;
    public int FireRateUp;
    public int RecoilTimeUp;
    public int ClipSizeUp;
    public int BulletSpeedUp;
    public int PenetrationUp;
    public int MultyShotUp;

}
