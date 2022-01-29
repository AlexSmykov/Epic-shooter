using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject HPBar;

    private void Start()
    {
        HPBar = GameObject.FindGameObjectWithTag("HpBarFill");
    }

    public void HealthBarUpdate(float HealthAmount)
    {
        HPBar.GetComponent<Image>().fillAmount = (float)Math.Round(HealthAmount, 2);
    }
}
