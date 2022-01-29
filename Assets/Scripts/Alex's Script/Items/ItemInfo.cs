using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Text ItemName;
    public Text ItemDescription;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateInfo(string Name, string Description)
    {
        ItemName.text = Name;
        ItemDescription.text = Description;
        animator.Play("New Animation");
    }
}
