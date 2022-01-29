using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float _Speed;
    [SerializeField] private GameObject _DashEffect;

    private Vector2 _Direction;
    private Rigidbody2D _Rigidbody;
    private Camera _Camera;
    private Animator _Animator;

    public static PlayerMoving singelton { get; private set; }

    public delegate void PlayerMov();
    public event PlayerMov Rolling;
    public event PlayerMov Running;

    private void Awake()
    {
        singelton = this;
    }

    void Start()
    {
        GunControl.singelton.GunLooksRight += ChangeRotationPlayer;
        _Camera = Camera.main;
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();

    }

    void Update()
    {
        _Direction.x = Input.GetAxisRaw("Horizontal");
        _Direction.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        if (_Direction.normalized.x != 0 || _Direction.normalized.y != 0)
            _Animator.SetBool("IsRunning", true);
        else
            _Animator.SetBool("IsRunning", false);


    }

    private void FixedUpdate()
    { 
        _Rigidbody.MovePosition(_Rigidbody.position + _Direction.normalized * _Speed * Time.fixedDeltaTime);

    }

    private void Dash()
    {
        if (_Direction.normalized.x != 0 || _Direction.normalized.y != 0)
        {
            Rolling?.Invoke();

            Instantiate(_DashEffect, transform.position, Quaternion.identity);

            _Rigidbody.AddForce((_Direction.normalized) * 10, ForceMode2D.Force);

        }
    }

    private void ChangeRotationPlayer(bool GunLooksRight)
    {
        if (GunLooksRight)
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

    }

}
