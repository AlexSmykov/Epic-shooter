using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float MaxHealth;
    public float Health;
    public int Coins;
    public int Keys;
    public int Materials;
    public float Speed;
    public int ChestPickupCount;
    public int StartWeaponIndex;

    [SerializeField] private float _Offset;

    private Vector2 _Direction;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;
    private Camera _Camera;
    public GameObject PlaceForGun;
    public GameObject UpEffect;
    public GameObject FloatingDamage;

    public GameObject CoinText;
    public GameObject KeyText;
    public GameObject MaterialText;

    public Gun[] PlayerGuns;
    public ObjectStorage Storage;

    private bool _IsGunLooksRight = true;

    public void Start()
    {
        MaxHealth = PlayerPrefs.GetFloat("MaxHealth", MaxHealth);
        Health = PlayerPrefs.GetFloat("Health", MaxHealth);
        GetComponent<HealthBar>().HealthBarUpdate(Health / MaxHealth);
        Coins = PlayerPrefs.GetInt("Coins", Coins);
        Keys = PlayerPrefs.GetInt("Keys", Keys);
        Materials = PlayerPrefs.GetInt("Materials", Materials);

        _Camera = Camera.main;
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();

        CoinText = GameObject.FindGameObjectWithTag("CoinText");
        KeyText = GameObject.FindGameObjectWithTag("KeyText");
        MaterialText = GameObject.FindGameObjectWithTag("MaterialText");
        UpdateResourcesText();

        Storage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ObjectStorage>();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MaxHealth", MaxHealth);
        PlayerPrefs.SetFloat("Health", Health);
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("Keys", Keys);
        PlayerPrefs.SetInt("Materials", Materials);
    }

    public void Update()
    {
        _Direction.x = Input.GetAxisRaw("Horizontal");
        _Direction.y = Input.GetAxisRaw("Vertical");

        LookAtCursor();
        if(Health <= 0)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Alex's scene");
        }
    }

    public void UpdateResourcesText()
    {
        CoinText.GetComponent<Text>().text = Coins.ToString();
        KeyText.GetComponent<Text>().text = Keys.ToString();
        MaterialText.GetComponent<Text>().text = Materials.ToString();
    }

    public void HealthChange(float HealthValue)
    {
        Health += HealthValue;
        if (Health > MaxHealth) 
        { 
            Health = MaxHealth;}
        GetComponent<HealthBar>().HealthBarUpdate(Health / MaxHealth);
        Vector2 DamagePosition = new Vector2(transform.position.x, transform.position.y + 1f);
        GameObject FloatDmg = Instantiate(FloatingDamage, DamagePosition, Quaternion.identity);
        FloatDmg.GetComponentInChildren<FloatingDamage>().Damage = HealthValue;
        FloatDmg.GetComponentInChildren<TextMesh>().fontSize += 150;
    }

    private void FixedUpdate()
    {
        //_Animator.SetBool("IsRunning", true);
        _Rigidbody.MovePosition(_Rigidbody.position + _Direction.normalized * Speed * Time.fixedDeltaTime);
    }

    private void LookAtCursor()
    {
        Vector3 Difference = _Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float RotationZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(RotationZ) > 90)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            PlaceForGun.transform.rotation = Quaternion.Euler(0f, -180f, -RotationZ + _Offset);
            PlaceForGun.transform.localScale = new Vector3(0.7f, -0.7f, 1);
            _IsGunLooksRight = false;
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            PlaceForGun.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ + _Offset);
            PlaceForGun.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            _IsGunLooksRight = true;
        }
    }

    public void WhenItemGrab(string ObjecctType)
    {

        Instantiate(UpEffect, transform);
    }

}
