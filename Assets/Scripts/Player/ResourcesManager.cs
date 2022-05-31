using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Менеджер ресурсов персонажа
/// </summary>
public class ResourcesManager : MonoBehaviour
{
    private int _Coins;
    public int Coins
    {
        get
        {
            return _Coins;
        }
        set
        {
            if (value > 0)
            {
                _Coins = value;
            }
            else
            {
                _Coins = 0;
            }
        }
    }
    private int _Keys;
    public int Keys
    {
        get
        {
            return _Keys;
        }
        set
        {
            if (value > 0)
            {
                _Keys = value;
            }
            else
            {
                _Keys = 0;
            }
        }
    }
    private int _Materials;
    public int Materials
    {
        get
        {
            return _Materials;
        }
        set
        {
            if (value > 0)
            {
                _Materials = value;
            }
            else
            {
                _Materials = 0;
            }
        }
    }

    private Text _CoinText;
    private Text _KeyText;
    private Text _MaterialText;

    private void Start()
    {
        _CoinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<Text>();
        _KeyText = GameObject.FindGameObjectWithTag("KeyText").GetComponent<Text>();
        _MaterialText = GameObject.FindGameObjectWithTag("MaterialText").GetComponent<Text>();
        UpdateResourcesText();
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Keys", 0);
        PlayerPrefs.SetInt("Materials", 0);
    }

    public void Load()
    {
        _Coins = PlayerPrefs.GetInt("Coins", _Coins);
        _Keys = PlayerPrefs.GetInt("Keys", _Keys);
        _Materials = PlayerPrefs.GetInt("Materials", _Materials);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Coins", _Coins);
        PlayerPrefs.SetInt("Keys", _Keys);
        PlayerPrefs.SetInt("Materials", _Materials);
    }

    public void UpdateResourcesText()
    {
        _CoinText.text = _Coins.ToString();
        _KeyText.text = _Keys.ToString();
        _MaterialText.text = _Materials.ToString();
    }
}
