using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour{
    [HideInInspector] public Item item;
    [HideInInspector] public Image image;

    public void InitialiseItem(Item newItem){
        image = GetComponent<Image>();

        item = newItem;
        image.sprite = newItem.itemIcon;
    }
}
