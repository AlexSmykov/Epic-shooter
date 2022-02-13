using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
	public string GunName;
	public float Offset;
	public GameObject Bullet;
	public Transform ShotPoint;
	private Text BulletText;
	private GameObject ReloadBar;
	private AudioSource audioSource;
	public AudioClip ReloadSound;
	public AudioClip ShotSound;

	public float StartBulletReloadTime;
	private float BulletReloadTime;
	public float StartClipRecoilTime;
	private float ClipRecoilTime;

	public int ClipSize;
	public int CurrentClipSize;
	public int BulletsCount;
	public int ShotBulletPenalty;
	public float Damage;
	public float BulletSpeed;

	public float Accuracy;

	public Sprite SecondLvl;
	public Sprite ThirdLvl;

	public float DamageBonusLevel;
	public float FireRateBonusLevel;
	public float BulletSpeedBonusLevel;
	public float ClipRecoilBonusLevel;
	public int ClipSizeBonusLevel;
	public int MultiShotBonusLevel;
	public int PenetrationBonusLevel;

	private bool GiveBullet;

	private void Awake()
    {
		BulletText = GameObject.Find("WeaponBulletText").GetComponent<Text>();
		ReloadBar = GameObject.FindGameObjectWithTag("ReloadBar");
		audioSource = GetComponent<AudioSource>();
		BulletTextUpdate();
	}

	public void Load()
	{
		CurrentClipSize = PlayerPrefs.GetInt(GunName + "CurrentClipSize", ClipSize);
		BulletsCount = PlayerPrefs.GetInt(GunName + "BulletsCount", BulletsCount);
		ClipSizeBonusLevel = PlayerPrefs.GetInt(GunName + "ClipSizeBonusLevel", ClipSizeBonusLevel);
		MultiShotBonusLevel = PlayerPrefs.GetInt(GunName + "MultiShotBonusLevel", MultiShotBonusLevel);
		PenetrationBonusLevel = PlayerPrefs.GetInt(GunName + "PenetrationBonusLevel", PenetrationBonusLevel);
		DamageBonusLevel = PlayerPrefs.GetFloat(GunName + "DamageBonusLevel", DamageBonusLevel);
		FireRateBonusLevel = PlayerPrefs.GetFloat(GunName + "FireRateBonusLevel", FireRateBonusLevel);
		BulletSpeedBonusLevel = PlayerPrefs.GetFloat(GunName + "BulletSpeedBonusLevel", BulletSpeedBonusLevel);
		ClipRecoilBonusLevel = PlayerPrefs.GetFloat(GunName + "ClipRecoilBonusLevel", ClipRecoilBonusLevel);
	}

	public void Save()
    {
		PlayerPrefs.SetInt(GunName + "CurrentClipSize", CurrentClipSize);
		PlayerPrefs.SetInt(GunName + "BulletsCount", BulletsCount);
		PlayerPrefs.SetInt(GunName + "ClipSizeBonusLevel", ClipSizeBonusLevel);
		PlayerPrefs.SetInt(GunName + "MultiShotBonusLevel", MultiShotBonusLevel);
		PlayerPrefs.SetInt(GunName + "PenetrationBonusLevel", PenetrationBonusLevel);
		PlayerPrefs.SetFloat(GunName + "DamageBonusLevel", DamageBonusLevel);
		PlayerPrefs.SetFloat(GunName + "FireRateBonusLevel", FireRateBonusLevel);
		PlayerPrefs.SetFloat(GunName + "BulletSpeedBonusLevel", BulletSpeedBonusLevel);
		PlayerPrefs.SetFloat(GunName + "ClipRecoilBonusLevel", ClipRecoilBonusLevel);
	}

    void Update()
	{
		Vector3 Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float rotZ = Mathf.Atan2(Difference.y, Difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ + Offset);

		if(ClipRecoilTime <= 0 && GiveBullet)
		{
			if (BulletsCount - ClipSize > 0)
			{
				BulletsCount -= ClipSize - CurrentClipSize + ClipSizeBonusLevel - 1;
				CurrentClipSize = ClipSize + ClipSizeBonusLevel - 1;
			}
			else
			{
				CurrentClipSize = BulletsCount;
				BulletsCount = 0;
			}
			BulletTextUpdate();
			GiveBullet = false;
			audioSource.PlayOneShot(ReloadSound);
		}

		if ((CurrentClipSize <= 0  || (Input.GetButton("r") && CurrentClipSize != ClipSize + ClipSizeBonusLevel - 1)) && ClipRecoilTime <= 0 && BulletsCount > 0)
        {
			ReloadGun();
		}

		if(Input.GetMouseButton(0) && BulletReloadTime <= 0 && ClipRecoilTime <= 0 && CurrentClipSize > 0)
		{
			for (int i = 0; i < MultiShotBonusLevel && CurrentClipSize > 0; i++)
			{
				Quaternion BulletRotate = Quaternion.Euler(0f, 0f, rotZ + Offset + Random.Range(-Accuracy, Accuracy));
				GameObject NewBullet = Instantiate(Bullet, ShotPoint.position, BulletRotate);
				NewBullet.GetComponent<Bullet>().Damage = Damage * (float)(1 + Mathf.Log(Mathf.Pow((float)((DamageBonusLevel - 1) * 0.75), 1.2f) / 5 + 1, 2));
				NewBullet.GetComponent<Bullet>().Speed = BulletSpeed * (float)(1 + Mathf.Pow(BulletSpeedBonusLevel - 1, 0.9f) / 6);
				NewBullet.GetComponent<Bullet>().PenetrationCount = PenetrationBonusLevel;
			}

			audioSource.PlayOneShot(ShotSound);
			CurrentClipSize -= ShotBulletPenalty;
			BulletReloadTime = StartBulletReloadTime * (float)(1 - Mathf.Pow(FireRateBonusLevel - 1, 0.75f) / 12);

			if (CurrentClipSize <= 0)
			{
				CurrentClipSize = 0;
			}

			BulletTextUpdate();
		}
        else
        {
			BulletReloadTime -= Time.deltaTime;
			ClipRecoilTime -= Time.deltaTime;

			if(GiveBullet)
			{
				ReloadBar.GetComponent<Image>().fillAmount = 1 - ClipRecoilTime / StartClipRecoilTime;
			}

		}
	}

	public void BulletTextUpdate()
    {
		string AllBullet = BulletsCount.ToString();

		if(BulletsCount > 999)
        {
			AllBullet = "999+";

		}

		if(gameObject.activeSelf)
		{
			BulletText.text = CurrentClipSize + "/" + AllBullet;
			UpdateReloadBar();
		}
	}

	private void ReloadGun()
	{
		ClipRecoilTime = StartClipRecoilTime;
		GiveBullet = true;
	}

    public void UpdateReloadBar()
	{
		ReloadBar.GetComponent<Image>().fillAmount = (float)CurrentClipSize / (float)(ClipSize + ClipSizeBonusLevel - 1);
	}
}