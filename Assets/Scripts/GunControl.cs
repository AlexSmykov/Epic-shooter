using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField] private float _Offset;

    private Camera _Camera;

    public static GunControl singelton { get; private set; }

    public delegate void GunAction();
    public event GunAction GunShooting;

    public delegate void GunRotation(bool IsGunLooksRight);
    public event GunRotation GunLooksRight;

    private void Awake()
    {
        singelton = this;
    }

    void Start()
    {
        _Camera = Camera.main;
    }

    void Update()
    {
        LookAtCursor();
        Shoot();
    }

    private void LookAtCursor()
    {
        Vector3 Difference = _Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float RotationZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(RotationZ) > 90)
        {
            transform.rotation = Quaternion.Euler(-180f, 0f, -RotationZ + _Offset);
            GunLooksRight?.Invoke(false);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + _Offset);
            GunLooksRight?.Invoke(true);
        }
    }
   
    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            GunShooting?.Invoke();

        }
    }
}
