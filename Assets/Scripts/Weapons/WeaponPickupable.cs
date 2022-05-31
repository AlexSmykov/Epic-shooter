using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для оружия, которое лежит на земле и его можно подобрать. 
/// В ходе рефакторинга ушли все поля и методы из класса, но решил не убирать его, т.к. связывает остальные два скрипта через RequireComponent
/// </summary>
[RequireComponent(typeof(WithId))]
[RequireComponent(typeof(Pickupable))]
public class WeaponPickupable : MonoBehaviour { }
