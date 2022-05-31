using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс игрока, в котором есть основыне параметры и описано его перемещение
/// </summary>
[RequireComponent(typeof(Resources))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(KeyboardController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _MaxHealth;
    public float MaxHealth
    {
        get
        {
            return _MaxHealth;
        }
        set
        {
            if (value > 0)
            {
                _MaxHealth = value;
            }
            else
            {
                _MaxHealth = 0;
            }
        }
    }
    private float _Health;
    [SerializeField] private float _Speed;
    public float Speed
    {
        get
        {
            return _Speed;
        }
        set
        {
            if (value > 0)
            {
                _Speed = value;
            }
            else
            {
                _Speed = 0;
            }
        }
    }
    [SerializeField] private int _ChestPickupCount;
    public int ChestPickupCount 
    { 
        get 
        { 
            return _ChestPickupCount;
        } 
        set 
        { 
            if (value > 0)
            {
                _ChestPickupCount = value;
            }
            else
            {
                _ChestPickupCount = 0;
            }
        } 
    }
    [SerializeField] private StateMachine.WeaponLink _StartWeapon;

    [SerializeField] private float _DashReloadTime;
    private float _CurrentDashReloadTime;

    [SerializeField] private float _Offset;
    [SerializeField] private GameObject _DashEffect;

    private Vector2 _Direction;
    private Rigidbody2D _Rigidbody;
    private Animator _animator;
    private Camera _Camera;
    private Transform _PlaceForGun;
    private HealthBar _HealthBar;
    [SerializeField] private GameObject _FloatingDamage;
    public GameObject FloatingDamage { get { return _FloatingDamage; } set { _FloatingDamage = value; } }

    public void Init()
    {
        _CurrentDashReloadTime = 0;

        _Camera = Camera.main;
        _Camera.gameObject.GetComponent<RoomChanger>().PlayerConnect(transform);
        Load();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _PlaceForGun = transform.GetChild(0);
        _HealthBar = GameObject.Find("PlayerHpBar").GetComponent<HealthBar>();

        WeaponSwitch weaponSwitch = transform.GetChild(0).GetComponent<WeaponSwitch>();
        weaponSwitch.Load();
        weaponSwitch.UnlockWeapon(_StartWeapon);
        UpdateHealthBar();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<Weapon>().Load();
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.SetFloat("MaxHealth", 150);
        PlayerPrefs.SetFloat("Health", 150);
    }

    public void ResetSavePosition()
    {
        PlayerPrefs.SetString("PlayerCords", new Vector3Int(0, 0, 0).ToString());
    }

    public void Load()
    {
        _MaxHealth = PlayerPrefs.GetFloat("MaxHealth", _MaxHealth);
        _Health = PlayerPrefs.GetFloat("Health", _MaxHealth);
        GetComponent<ResourcesManager>().Load();

        string cords = PlayerPrefs.GetString("PlayerCords", new Vector3(0, 0, 0).ToString());
        string[] cords_separate = cords.Split(',');
        transform.position = new Vector3(
            float.Parse(cords_separate[0].Substring(1, cords_separate[0].Length - 1), CultureInfo.InvariantCulture),
            float.Parse(cords_separate[1].Substring(1, cords_separate[1].Length - 1), CultureInfo.InvariantCulture),
            float.Parse(cords_separate[2].Substring(1, cords_separate[2].Length - 2), CultureInfo.InvariantCulture));
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MaxHealth", _MaxHealth);
        PlayerPrefs.SetFloat("Health", _Health);
        GetComponent<ResourcesManager>().Save();
        PlayerPrefs.SetString("PlayerCords", transform.position.ToString());
    }

    private void Update()
    {
        LookAtCursor();
        if(_Health <= 0)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Alex's scene");
        }

        _CurrentDashReloadTime -= Time.deltaTime;

        _Direction.x = Input.GetAxisRaw("Horizontal");
        _Direction.y = Input.GetAxisRaw("Vertical");
        if (_Direction.x != 0 || _Direction.y != 0)
        {
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

    public IEnumerator Dash()
    {
        if ((_Direction.x != 0 || _Direction.y != 0) && _CurrentDashReloadTime < 0)
        {
            Instantiate(_DashEffect, transform.position, Quaternion.identity);
            for (int i = 0; i < 100; i++)
            {
                _Rigidbody.AddForce(_Direction.normalized * 60, ForceMode2D.Force);
                _CurrentDashReloadTime = _DashReloadTime;
                yield return new WaitForSeconds(0.0005f);
            }
        }
    }

    private void UpdateHealthBar()
    {
        _HealthBar.Init();
        _HealthBar.HealthBarUpdate(_Health / _MaxHealth);
    }

    public void HealthChange(float HealthValue)
    {
        float OldHealth = _Health;
        _Health += HealthValue;

        if (_Health > _MaxHealth) 
        {
            _Health = _MaxHealth;
        }

        UpdateHealthBar();
        Vector2 DamagePosition = new Vector2(transform.position.x, transform.position.y + 1f);
        GameObject FloatDmg = Instantiate(_FloatingDamage, DamagePosition, Quaternion.identity);
        FloatDmg.GetComponentInChildren<FloatingDamage>().Damage = (_Health - OldHealth);
        FloatDmg.GetComponentInChildren<TextMesh>().fontSize += 150;
    }

    private void FixedUpdate()
    {
        _Rigidbody.MovePosition(_Rigidbody.position + _Direction.normalized * _Speed * Time.fixedDeltaTime);
    }

    private void LookAtCursor()
    {
        Vector3 Difference = _Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float RotationZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(RotationZ) > 90)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _PlaceForGun.transform.rotation = Quaternion.Euler(0f, -180f, -RotationZ + _Offset);
            _PlaceForGun.transform.localScale = new Vector3(0.7f, -0.7f, 1);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _PlaceForGun.rotation = Quaternion.Euler(0f, 0f, RotationZ + _Offset);
            _PlaceForGun.localScale = new Vector3(0.7f, 0.7f, 1);
        }
    }
}
