using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpriteChanger : MonoBehaviour
{
    public Sprite OpenDoorSprite;

    public void ChangeSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = OpenDoorSprite;
    }
}
