using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для полоски с жизнями
/// </summary>
public class HealthBar : MonoBehaviour
{
    private Image _HpFillImage;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _HpFillImage = transform.Find("Fill").GetComponent<Image>();
    }

    public void HealthBarUpdate(float HealthAmount)
    {
        _HpFillImage.fillAmount = (float)Math.Round(HealthAmount, 2);
    }
}
