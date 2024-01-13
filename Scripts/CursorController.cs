using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public static CursorController instance;
    public Texture2D crossTexture, circleTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    public void ActivateCrossCursor() {
        Cursor.SetCursor(crossTexture, new Vector2(crossTexture.width / 2, crossTexture.height / 2), cursorMode);
    }

    public void ActivateCircleCursor() {
        Cursor.SetCursor(circleTexture, new Vector2(crossTexture.width / 2, crossTexture.height / 2), cursorMode);
    }
}
