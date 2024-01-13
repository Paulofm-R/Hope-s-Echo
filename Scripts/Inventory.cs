using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    public List<Item> Items = new();
    public GameObject[] inventorySlots;
    public GameObject InventoryItemIconPrefab;

    private int _selectedSlot = -1;   

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    public void ChangeSelectedSlot(int newValue) {
        if (newValue < Items.Count) {
            if (_selectedSlot >= 0)
                InventorySlots.instance.Deselect(inventorySlots[_selectedSlot]);

            if (newValue != _selectedSlot) {
                _selectedSlot = newValue;
                Item item = GetSelectedItem();
                if (PuzzleController.instance.LookPaper(item) || PuzzleController.instance.UseTimeMachine(item))
                    _selectedSlot = -1;
                else
                    InventorySlots.instance.Select(inventorySlots[newValue]);
            } else
                _selectedSlot = -1;
        }
        
    }

    public void Add(Item item) {
        for (int i = 0; i < inventorySlots.Length; i++){
            GameObject slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null){
                Items.Add(item);
                ListItems(item, slot);
                return;
            }
        }
    }

    public void Remove(Item item) {
        // In cases where no item is selected
        if (_selectedSlot < 0) {
            for (int i = 0; i < Items.Count; i++) {
                if (Items[i] == item) {
                    _selectedSlot = i;
                }
            }
        }

        if (_selectedSlot >= 0){
            Items.Remove(item);
            GameObject slot = inventorySlots[_selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            Destroy(itemInSlot.gameObject);

            InventorySlots.instance.Deselect(inventorySlots[_selectedSlot]);
            UpdateSlots();
            _selectedSlot = -1;
        } else
            Debug.LogWarning("Item not found in inventory: " + item.name);
    }

    private void ListItems(Item item, GameObject slot) {
        GameObject newItemGo = Instantiate(InventoryItemIconPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    } 

    public Item GetSelectedItem() {
        if (_selectedSlot >= 0){
            GameObject slot = inventorySlots[_selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                return itemInSlot.item;
            }
            return null; 
        }
        return null;
    }

    public void UpdateSlots() {
        // Clear all slots
        foreach (GameObject slot in inventorySlots) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                Destroy(itemInSlot.gameObject);
            }
        }

        // Recreate slots with updated items
        for (int i = 0; i < Items.Count; i++) {
            GameObject slot = inventorySlots[i];
            ListItems(Items[i], slot);
        }
    }
}
