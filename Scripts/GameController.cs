using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;

    [Serializable]
    public struct Door {
        public string name;
        public GameObject door;
        public GameObject goTo;
        public GameObject activateCamera;
        public GameObject disableCamera;
    };

    [Serializable]
    public struct Items {
        public string name;
        public GameObject Object;
        public Item item;
    }

    [Serializable]
    public struct TravelTimeCoor {
        public string name;
        public GameObject goTo;
        public GameObject activateCamera;
        public GameObject disableCamera;
    }

    public Door[] Doors;
    public Items[] ItemsList;
    public TravelTimeCoor[] TravelTimeCoors;


    public GameObject TextsObject;
    public GameObject FinishScreen;
    public GameObject PauseMenu;
    public TMP_Text Text;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start() {
        CursorController.instance.ActivateCrossCursor();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)){  
            PauseMenu.SetActive(true);
        }
    }

    public void ChangeDivision(string doorName, GameObject player){
        Door door = Doors.FirstOrDefault(d => d.door.name == doorName);

        Vector3 newPosition = player.transform.position;
        newPosition.x = door.goTo.transform.position.x;
        newPosition.y = door.goTo.transform.position.y;
        player.transform.position = newPosition;

        door.activateCamera.SetActive(true);
        door.disableCamera.SetActive(false);

        PlayerController.instance.StoppedPlayer();     
    }

    public void ShowDialogues (string text, float delay) {
        Text.text = text;
        TextsObject.SetActive(true);

        StartCoroutine(ToOmitDialogues(delay));
    }

    public IEnumerator ToOmitDialogues (float delay) {
        yield return new WaitForSeconds(delay);

        TextsObject.SetActive(false);
    }

    public void ItemPickup (GameObject gameObject) {
        Items item = ItemsList.FirstOrDefault(i => (i.Object != null && i.Object.name == gameObject.name));

        Inventory.instance.Add(item.item);
        Destroy(gameObject);
    }

    public Item GetItem(string action) {
        if (action != "") {
            Items item = ItemsList.FirstOrDefault(i => i.name == action);        
            return item.item;
        }
        return null;
    }

    public void UnlockDoor(string doorName) {
        Door door = Doors.FirstOrDefault(d => d.name == doorName);

        door.door.tag = "ChangeDivision";
    }

    public void DestroyItem(string itemName) {
        foreach (var item in ItemsList){
            if (item.name == itemName)
                Destroy(item.Object);
        }
    }

    public void TravelTime() {
        PlayerController player = PlayerController.instance.GetComponent<PlayerController>();

        Vector3 newPosition = player.transform.position;
        newPosition.y = TravelTimeCoors[0].goTo.transform.position.y;
        player.transform.position = newPosition;

        TravelTimeCoors[0].activateCamera.SetActive(true);
        TravelTimeCoors[0].disableCamera.SetActive(false);

        StartCoroutine(TalksController.instance.FinishDemoGame());

        PlayerController.instance.StoppedPlayer();  
    }

    public void FinishGame(){
        FinishScreen.SetActive(true);
    }

    public void ClosePauseMenu() {
        PauseMenu.SetActive(false);
    }

    public void BackMenu(){
        SceneManager.LoadScene("Menu");
    }
}
