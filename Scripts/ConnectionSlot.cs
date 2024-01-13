using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionSlot : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        DraggableWires wire = eventData.pointerDrag.GetComponent<DraggableWires>();

        if (wire.gameObject.CompareTag(gameObject.tag)){
            wire.parentAfterDrag = transform;
            PuzzleController.instance.TimeMachineConnections();
            wire.isConnected = true;
        }
            
    }
}
