using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private void Awake()
    {
        var tex = Resources.Load<Texture2D>("UI/光标");
        UnityEngine.Cursor.SetCursor(tex, new Vector2(0, 0), CursorMode.ForceSoftware);
    }
}
