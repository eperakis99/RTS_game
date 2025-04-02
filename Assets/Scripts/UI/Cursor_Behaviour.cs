using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Behaviour : MonoBehaviour
{

    public Texture2D idleCursor;
    public Texture2D pointerCursor;
    private float timer = 0.0f; //number of frames the pointerCursor is active
    private bool block = false;
    private int ticks = 0;

    private void Awake()
    {
        ApplyCursor(idleCursor);
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void ApplyCursor(Texture2D cursor)
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

    }

    private void Update()
    {
        block = false;
        ticks++;

        if (Input.GetMouseButtonDown(0))
        {
            if (timer < 0.9f)
            {
                ticks = 0;
                Cursor.visible = false;
            }
            Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
            block = true;

            
        }

        if (ticks > 5)
            Cursor.visible = true;

        timer += Time.smoothDeltaTime ;
        if(timer >= 1.0f && !block)
        {

            timer = 0.0f;
            Cursor.SetCursor(idleCursor, Vector2.zero, CursorMode.Auto);
            
            
        }
    }
}
