using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

	public enum ShakeMode { OnlyX, OnlyY, OnlyZ, XY, XZ, XYZ };

	private static Transform _Transform;
	private static float elapsed, i_Duration, i_Power, percentComplete;
	private static ShakeMode i_Mode;
	private static Vector3 originalPos;

    private void Awake()
    {
		PlayerMoving.singelton.Rolling += MediumShake;
    }

    void Start()
	{
		GunControl.singelton.GunShooting += LittleShake;
		percentComplete = 1;
		_Transform = GetComponent<Transform>();
	}

	public static void LittleShake()
	{
		(float duration, float power) = (Random.Range(0.01f, 0.05f), Random.Range(0.005f, 0.01f));
		if (percentComplete == 1) originalPos = _Transform.localPosition;
		i_Mode = ShakeMode.XYZ;
		elapsed = 0;
		i_Duration = duration;
		i_Power = power;
	}

	public static void MediumShake()
	{
		(float duration, float power) = (Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f));
		if (percentComplete == 1) originalPos = _Transform.localPosition;
		i_Mode = ShakeMode.XYZ;
		elapsed = 0;
		i_Duration = duration;
		i_Power = power;
	}

	public static void Shake(float duration, float power, ShakeMode mode)
	{
		if (percentComplete == 1) originalPos = _Transform.localPosition;
		i_Mode = mode;
		elapsed = 0;
		i_Duration = duration;
		i_Power = power;
	}

	void Update()
	{
		if (elapsed < i_Duration)
		{
			elapsed += Time.deltaTime;
			percentComplete = elapsed / i_Duration;
			percentComplete = Mathf.Clamp01(percentComplete);
			Vector3 rnd = Random.insideUnitSphere * i_Power * (1f - percentComplete);

			switch (i_Mode)
			{
				case ShakeMode.XYZ:
					_Transform.localPosition = originalPos + rnd;
					break;
				case ShakeMode.OnlyX:
					_Transform.localPosition = originalPos + new Vector3(rnd.x, 0, 0);
					break;
				case ShakeMode.OnlyY:
					_Transform.localPosition = originalPos + new Vector3(0, rnd.y, 0);
					break;
				case ShakeMode.OnlyZ:
					_Transform.localPosition = originalPos + new Vector3(0, 0, rnd.z);
					break;
				case ShakeMode.XY:
					_Transform.localPosition = originalPos + new Vector3(rnd.x, rnd.y, 0);
					break;
				case ShakeMode.XZ:
					_Transform.localPosition = originalPos + new Vector3(rnd.x, 0, rnd.z);
					break;
			}
		}
	}
}
