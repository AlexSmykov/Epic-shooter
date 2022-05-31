using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Класс Оружия, со всеми параметрами
/// </summary>
[RequireComponent(typeof(WithId))]
public class Weapon : MonoBehaviour
{
	[SerializeField] private GameObject _Bullet;
	private Transform _ShotPoint;
	private Text _BulletText;
	private GameObject _ReloadBar;
	private AudioSource _audioSource;
	[SerializeField] private AudioClip _ReloadSound;
	[SerializeField] private AudioClip _ShotSound;

	[SerializeField] private float _BulletReloadTime;
	private float _CurrentBulletReloadTime;
	[SerializeField] private float _ClipRecoilTime;
	private float _CurrentClipRecoilTime;
	[SerializeField] private int _ClipSize;
	private int _CurrentClipSize;
	[SerializeField] private int _BulletsCount;
	public int BulletsCount { get { return _BulletsCount; } set { _BulletsCount = value; } }
	[SerializeField] private int _ShotBulletPenalty;
	[SerializeField] private float _Damage;
	[SerializeField] private float _BulletSpeed;

	[SerializeField] private float _Accuracy;

	private SpriteRenderer _Sprite;
	[SerializeField] private Sprite _FirstLvl;
	[SerializeField] private Sprite _SecondLvl;
	[SerializeField] private Sprite _ThirdLvl;
	[SerializeField] private int _SecondLvlScore;
	[SerializeField] private int _ThirdLvlScore;

	[SerializeField] private float _DamageBonusLevel;
	public float DamageBonusLevel { get { return _DamageBonusLevel; } set { _DamageBonusLevel = value; } }
	[SerializeField] private float _FireRateBonusLevel;
	public float FireRateBonusLevel { get { return _FireRateBonusLevel; } set { _FireRateBonusLevel = value; } }
	[SerializeField] private float _BulletSpeedBonusLevel;
	public float BulletSpeedBonusLevel { get { return _BulletSpeedBonusLevel; } set { _BulletSpeedBonusLevel = value; } }
	[SerializeField] private float _ClipRecoilBonusLevel;
	public float ClipRecoilBonusLevel { get { return _ClipRecoilBonusLevel; } set { _ClipRecoilBonusLevel = value; } }
	[SerializeField] private int _ClipSizeBonusLevel;
	public int ClipSizeBonusLevel { get { return _ClipSizeBonusLevel; } set { _ClipSizeBonusLevel = value; } }
	[SerializeField] private int _MultiShotBonusLevel;
	public int MultiShotBonusLevel { get { return _MultiShotBonusLevel; } set { _MultiShotBonusLevel = value; } }
	[SerializeField] private int _PenetrationBonusLevel;
	public int PenetrationBonusLevel { get { return _PenetrationBonusLevel; } set { _PenetrationBonusLevel = value; } }

	[SerializeField] private bool _BangAfterDestroy;
	[SerializeField] private float _BangRadius;
	[SerializeField] private float _BangDamage;
	private bool _GiveBullet;
	[HideInInspector] public bool Unlocked;

	private void Awake()
    {
		Init();
    }

	public void Init()
	{
		_BulletText = GameObject.Find("WeaponBulletText").GetComponent<Text>();
		_ReloadBar = GameObject.FindGameObjectWithTag("ReloadBar");
		_audioSource = GetComponent<AudioSource>();
		_Sprite = GetComponent<SpriteRenderer>();
		_ShotPoint = transform.GetChild(0);
		BulletTextUpdate();
		UpdateSkin();
	}

	public void Load()
	{
		_CurrentClipSize = PlayerPrefs.GetInt(gameObject.name + "CurrentClipSize", _ClipSize);
		_BulletsCount = PlayerPrefs.GetInt(gameObject.name + "BulletsCount", _BulletsCount);
		_ClipSizeBonusLevel = PlayerPrefs.GetInt(gameObject.name + "ClipSizeBonusLevel", _ClipSizeBonusLevel);
		_MultiShotBonusLevel = PlayerPrefs.GetInt(gameObject.name + "MultiShotBonusLevel", _MultiShotBonusLevel);
		_PenetrationBonusLevel = PlayerPrefs.GetInt(gameObject.name + "PenetrationBonusLevel", _PenetrationBonusLevel);
		_DamageBonusLevel = PlayerPrefs.GetFloat(gameObject.name + "DamageBonusLevel", _DamageBonusLevel);
		_FireRateBonusLevel = PlayerPrefs.GetFloat(gameObject.name + "FireRateBonusLevel", _FireRateBonusLevel);
		_BulletSpeedBonusLevel = PlayerPrefs.GetFloat(gameObject.name + "BulletSpeedBonusLevel", _BulletSpeedBonusLevel);
		_ClipRecoilBonusLevel = PlayerPrefs.GetFloat(gameObject.name + "ClipRecoilBonusLevel", _ClipRecoilBonusLevel);
		BulletTextUpdate();
	}

	public void Save()
    {
		PlayerPrefs.SetInt(gameObject.name + "CurrentClipSize", _CurrentClipSize);
		PlayerPrefs.SetInt(gameObject.name + "BulletsCount", _BulletsCount);
		PlayerPrefs.SetInt(gameObject.name + "ClipSizeBonusLevel", _ClipSizeBonusLevel);
		PlayerPrefs.SetInt(gameObject.name + "MultiShotBonusLevel", _MultiShotBonusLevel);
		PlayerPrefs.SetInt(gameObject.name + "PenetrationBonusLevel", _PenetrationBonusLevel);
		PlayerPrefs.SetFloat(gameObject.name + "DamageBonusLevel", _DamageBonusLevel);
		PlayerPrefs.SetFloat(gameObject.name + "FireRateBonusLevel", _FireRateBonusLevel);
		PlayerPrefs.SetFloat(gameObject.name + "BulletSpeedBonusLevel", _BulletSpeedBonusLevel);
		PlayerPrefs.SetFloat(gameObject.name + "ClipRecoilBonusLevel", _ClipRecoilBonusLevel);
	}

    void Update()
	{
		Vector3 Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float rotZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

		// Далее находится большая и сложная логика стрельбы и перезарядки оружия, желательно, пофиксить бы
		if(_CurrentClipRecoilTime <= 0 && _GiveBullet)
		{
			if (_BulletsCount - (_ClipSize + _ClipSizeBonusLevel - 1) > 0)
			{
				_BulletsCount -= _ClipSize + _ClipSizeBonusLevel - 1 - _CurrentClipSize;
				_CurrentClipSize = _ClipSize + _ClipSizeBonusLevel - 1;
			}
			else
			{
				_CurrentClipSize = _BulletsCount;
				_BulletsCount = 0;
			}
			BulletTextUpdate();
			_GiveBullet = false;
			_audioSource.PlayOneShot(_ReloadSound);
		}

		if ((_CurrentClipSize <= 0  || (Input.GetButton("r") && _CurrentClipSize != _ClipSize + _ClipSizeBonusLevel - 1)) && _CurrentClipRecoilTime <= 0 && _BulletsCount > 0)
        {
			ReloadGun();
		}

		if(Input.GetMouseButton(0) && _CurrentBulletReloadTime <= 0 && _CurrentClipRecoilTime <= 0 && _CurrentClipSize > 0)
		{
			//Происходит выстрел
			for (int i = 0; i < _MultiShotBonusLevel && _CurrentClipSize > 0; i++)
			{
				GameObject NewBullet = Instantiate(_Bullet, _ShotPoint.position, Quaternion.Euler(0f, 0f, rotZ + Random.Range(-_Accuracy, _Accuracy)));
				Bullet bullet = NewBullet.GetComponent<Bullet>();
				bullet.BangAfterDestroy = _BangAfterDestroy;
				bullet.Damage = _Damage * (float)(1 + Mathf.Log(Mathf.Pow((float)((_DamageBonusLevel - 1) * 0.75), 1.2f) / 5 + 1, 2));
				bullet.Speed = _BulletSpeed * (float)(1 + Mathf.Pow(_BulletSpeedBonusLevel - 1, 0.9f) / 6);
				bullet.PenetrationCount = _PenetrationBonusLevel;
				bullet.BangRadius = _BangRadius;
				bullet.BangDamage = _BangDamage;
			}

			_audioSource.PlayOneShot(_ShotSound);
			_CurrentClipSize -= _ShotBulletPenalty;
			_CurrentBulletReloadTime = _BulletReloadTime * (float)(1 - (_FireRateBonusLevel - 1) / (_FireRateBonusLevel + 5));

			if (_CurrentClipSize <= 0)
			{
				_CurrentClipSize = 0;
			}

			BulletTextUpdate();
		}
        else
        {
			_CurrentBulletReloadTime -= Time.deltaTime;
			_CurrentClipRecoilTime -= Time.deltaTime * (1 + _ClipRecoilBonusLevel / 4);

			if(_GiveBullet)
			{
				_ReloadBar.GetComponent<Image>().fillAmount = 1 - _CurrentClipRecoilTime / _ClipRecoilTime;
			}
		}
	}

	/// <summary>
	/// Обновление скина оружия, при увеличении уровня оружия (суммарноо бонуса улучшений)
	/// </summary>
	public void UpdateSkin()
    {

		int Level = (int) 
			(_DamageBonusLevel + 
			_FireRateBonusLevel + 
			_BulletSpeedBonusLevel + 
			_ClipRecoilBonusLevel + 
			_ClipSizeBonusLevel + 
			_MultiShotBonusLevel + 
			_PenetrationBonusLevel);

		if (Level >= _ThirdLvlScore)
		{
			_Sprite.sprite = _ThirdLvl;
		}
		else if (Level >= _SecondLvlScore)
        {
			_Sprite.sprite = _SecondLvl;
		}
        else
		{
			_Sprite.sprite = _FirstLvl;
		}
	}

	public void BulletTextUpdate()
    {
		string AllBullet = _BulletsCount.ToString();

		if(_BulletsCount > 999)
        {
			AllBullet = "999+";

		}

		if(gameObject.activeSelf && _BulletText != null)
		{
			_BulletText.text = _CurrentClipSize + "/" + AllBullet;
			UpdateReloadBar();
		}
	}

	private void ReloadGun()
	{
		_CurrentClipRecoilTime = _ClipRecoilTime;
		_GiveBullet = true;
	}

    public void UpdateReloadBar()
	{
		_ReloadBar.GetComponent<Image>().fillAmount = (float)_CurrentClipSize / (float)(_ClipSize + _ClipSizeBonusLevel - 1);
	}
}