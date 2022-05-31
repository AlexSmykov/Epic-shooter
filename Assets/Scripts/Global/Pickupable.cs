using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject _PickupEffect;
    [SerializeField] private Factory.BaseObjects _PickupType;
    public Factory.BaseObjects PickupType { get { return _PickupType; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _OnPickup(collision);
        }
    }

    private void _OnPickup(Collider2D Player)
    {
        Instantiate(_PickupEffect, transform.position, Quaternion.identity);
        Player.GetComponent<PickupController>().OnPickup(gameObject);
        GetComponent<WithId>().OnGrab();
        Destroy(gameObject);
    }
}
