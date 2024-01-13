using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMouse : MonoBehaviour {
    void OnMouseEnter() {
        CursorController.instance.ActivateCircleCursor();
    }

    void OnMouseExit() {
        CursorController.instance.ActivateCrossCursor();
    }
}
