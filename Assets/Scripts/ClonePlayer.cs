using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlayer : MonoBehaviour
{/*
    [SerializeField] private float _Speed;
    [SerializeField] private float _Offset;
    [SerializeField] private GameObject _GunObject;
    [SerializeField] private GameObject _DashEffect;

    private Vector2 _Direction;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;
    private Camera _Camera;

    private bool _IsGunLooksRight = true;

    public static ClonePlayer singelton { get; private set; }

    public delegate void RollingAction();
    public event RollingAction Rolling;
    public event RollingAction Shoting;

    enum State
    {
        Running,
        Idle,
        Rolling,
    }
    State PersonState;

    private void Awake()
    {
        singelton = this;
    }

    void Start()
    {
        PersonState = State.Idle;
        _Camera = Camera.main;
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        _Direction.x = Input.GetAxisRaw("Horizontal");
        _Direction.y = Input.GetAxisRaw("Vertical");

        LookAtCursor();
        //ChangeRotationPlayer();
        //Running();
        //Dash();
        //Shoot();
    }
    /*
    private void FixedUpdate()
    {
        _Rigidbody.MovePosition(_Rigidbody.position + _Direction.normalized * _Speed * Time.fixedDeltaTime);

    }
    */

    /*
    private void Running()
    {
        if (_Direction.x != 0 || _Direction.y != 0)
        {
            PersonState = State.Running;
            _Animator.SetBool("IsRunning", true);

        }
        else
        {
            PersonState = State.Idle;
            _Animator.SetBool("IsRunning", false);

        }
    }
    */
    

    /*
    private void ChangeRotationPlayer()
    {
        if (_IsGunLooksRight == false)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if (_IsGunLooksRight == true)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        }

    }
    */
    /*

    private void LookAtCursor()
    {
        Vector3 Difference = _Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float RotationZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(RotationZ) > 90)
        {
            _GunObject.transform.rotation = Quaternion.Euler(-180f, 0f, -RotationZ + _Offset);
            _IsGunLooksRight = false;
        }
        else
        {
            _GunObject.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + _Offset);
            _IsGunLooksRight = true;
        }
    }
    /*
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_Direction.normalized.x != 0 || _Direction.normalized.y != 0)
            {
                _GunObject.SetActive(false);

                Rolling?.Invoke(Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f));

                Instantiate(_DashEffect, transform.position, Quaternion.identity);
                PersonState = State.Rolling;

                _Rigidbody.AddForce((_Direction.normalized) * 10, ForceMode2D.Force);

            }

            _GunObject.SetActive(true);
            PersonState = State.Idle;
        }
    }
    private void Shoot()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Shoting.Invoke(Random.Range(0.01f, 0.05f), Random.Range(0.005f, 0.01f));

        }
    }    
    */
}
