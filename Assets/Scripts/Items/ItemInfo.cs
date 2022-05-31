using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// При подборе улучшения, сверху вылезает менюшка, показывающая название и описание предмета
/// </summary>
public class ItemInfo : MonoBehaviour
{
    private Text _ItemName;
    private Text _ItemDescription;
    private Animator _Animator;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
        _ItemName = transform.Find("ItemName").GetComponent<Text>();
        _ItemDescription = transform.Find("ItemDescription").GetComponent<Text>();
    }

    public void ActivateInfo(string Name, string Description)
    {
        _ItemName.text = Name;
        _ItemDescription.text = Description;
        _Animator.Play("New Animation");
    }
}
