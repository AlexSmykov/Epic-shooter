using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс подбираемого предмета
/// </summary>
[RequireComponent(typeof(WithId))]
[RequireComponent(typeof(Pickupable))]
public class Pickups : MonoBehaviour
{
    public int PickupCount;

    private float _PickupIdleEffectTime;
    private float _StartPickupIdleEffectTime = 1.5f;

    [SerializeField] private Factory.PickupTypes _Type;
    [HideInInspector] public Factory.PickupTypes Type { get => _Type; }

    [SerializeField] private GameObject _PickupIdleEffect;

    private void Update()
    {
        if(_PickupIdleEffectTime < 0)
        {
            _PickupIdleEffectTime = Random.Range(_StartPickupIdleEffectTime * 0.8f, _StartPickupIdleEffectTime * 1.2f);
            Instantiate(_PickupIdleEffect, transform.position, Quaternion.identity);
        }
        else
        {
            _PickupIdleEffectTime -= Time.deltaTime;
        }
    }
}
