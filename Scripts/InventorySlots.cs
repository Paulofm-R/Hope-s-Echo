using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour {
    public static InventorySlots instance;

    public Image image;
    public Color selectedColor, notSelectedColor;

    void Awake(){
        DontDestroyOnLoad (this);

        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    public void Select(GameObject slot) {
        image = slot.GetComponent<Image>();
        image.color = selectedColor;
    }

    public void Deselect(GameObject slot) {
        image = slot.GetComponent<Image>();
        image.color = notSelectedColor;
    }
}
