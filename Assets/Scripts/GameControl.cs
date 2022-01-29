using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] Texture2D AimTexture;
    
    void Start()
    {
        Cursor.SetCursor(AimTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    void Update()
    {
        
    }
}
