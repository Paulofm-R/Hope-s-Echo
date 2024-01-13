using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using TMPro;

public class PuzzleController : MonoBehaviour {
    public static PuzzleController instance;

    [Serializable]
    public struct Interaction {
        public string InteractionName;
        public GameObject Object;
        public Item item;
    };

    public Interaction[] Interactions;
    public GameObject KeypadAccessSytem;

    public GameObject Paper;
    public GameObject WhiteBoard;
    public GameObject TimeMachineAssembly;
    public TMP_Text PaperText;
    

    private int _piecesAcquired = 0;
    private int _assembledParts = 0;
    private int _connectionsMade = 0;
    private List<int> _randomNumbers = new();
    private List<char> _puzzleText = new();

    private bool _hasCatEaten = false;


    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start() {
        RandomNumber();
        ConvertNumbersToLetters();
        SetPaperText();
    }

    private void RandomNumber() {
        Random random = new();

        for (int i = 0; i < 6; i++) {
            int number;

            // To check if you are already on the fifth number to prevent you from ending up with six numbers
            if (i == 5)
                number = random.Next(1, 10);
            else 
                number = random.Next(1, 27);
            
            _randomNumbers.Add(number);

            // If the number has two digits
            if (number >= 10)
                i++;
        }
    }

    private void ConvertNumbersToLetters() {
        foreach  (int num in _randomNumbers){
            char letter = (char)((int)'A' + num - 1);
            _puzzleText.Add(letter);
        }
    }

    private void SetPaperText() {
        foreach (char text in _puzzleText) {
            PaperText.text += text;
        }
    }

    public IEnumerator Interact(GameObject gameObject) {
        Item itemSelected = Inventory.instance.GetSelectedItem();

        Interaction interaction = Array.Find(Interactions, i => i.Object == gameObject && i.item == itemSelected);

        if (interaction.InteractionName != null) {
            Item item = null;

            switch (interaction.InteractionName) {
                case "Feed the cat":
                    item = GameController.instance.GetItem("Green Card");
                    Inventory.instance.Add(item);
                    Inventory.instance.Remove(itemSelected);
                    GameController.instance.ShowDialogues("Here's your food!", 2f);
                    _hasCatEaten = true;

                    break;
                case "Unlock at the door":
                    string doorName = "";
                    if (interaction.Object.name == "Card reader (green)")
                        doorName = "Hall to Green Room";

                    GameController.instance.UnlockDoor(doorName);
                    Inventory.instance.Remove(itemSelected);
                    break;
                case "Open Bookshelfs":
                    string bookshelfsName = "";

                    if (interaction.Object.name == "Wardrobes 1") {
                        bookshelfsName = "Piece 1";
                    } else if (interaction.Object.name == "Wardrobes 6") {
                        bookshelfsName = "Piece 2";
                    } else if (interaction.Object.name == "Wardrobes 4") {
                        bookshelfsName = "Piece 3";
                    } else if (interaction.Object.name == "Wardrobes 8") {
                        bookshelfsName = "Piece 4";
                    }

                    if (bookshelfsName != ""){
                        item = GameController.instance.GetItem(bookshelfsName);
                        Inventory.instance.Add(item);
                        _piecesAcquired++;

                        if (_piecesAcquired == 4){
                            Inventory.instance.Remove(itemSelected);
                            TalksController.instance.Workbench();
                        }

                        gameObject.tag = "Bookshelfs";
                    }
                    break;
                case "Put the Pieces Together":
                    _assembledParts++;
                    if (_assembledParts >= 4){
                        item = GameController.instance.GetItem("Compressor");
                        Inventory.instance.Add(item);
                        TalksController.instance.CompressorFinished();
                    }

                    Inventory.instance.Remove(itemSelected);                  
                    break;
                case "Time Machine": 
                    Inventory.instance.Remove(itemSelected);
                    TimeMachineAssembly.SetActive(true);
                    break;
                default:
                    break;
            }
        } else {
            if (gameObject.name == "Cat-2")
                StartCoroutine(TalksController.instance.TalkCat(_hasCatEaten));
            else if (gameObject.name == "Wardrobes 1" || gameObject.name == "Wardrobes 6" || gameObject.name == "Wardrobes 4" || gameObject.name == "Wardrobes 8")
                TalksController.instance.Bookshelfs(true);
        }

        

        yield return new WaitForSeconds(1);
        PlayerController.instance.hasInteracted = false;
    }

    public void NPCInteract(bool hasPaper) {
        if (!hasPaper){
            Item item = GameController.instance.GetItem("Paper");
            Inventory.instance.Add(item);
        }
    }

    public bool LookPaper(Item item) {
        if (item.itemName == "Paper"){
            Paper.SetActive(true);
            return true;
        }
        return false;
    }

    public bool UseTimeMachine(Item item) {
        if (item.itemName == "TimeMachine"){
            GameController.instance.TravelTime();
            return true;
        }
        return false;
    }

    public void DeactivateIfActivated(){
        if (KeypadAccessSytem.activeSelf)
            KeypadAccessSytem.SetActive(false);
        
        if (Paper.activeSelf)
            Paper.SetActive(false);

        if (WhiteBoard.activeSelf)
            WhiteBoard.SetActive(false);
    }

    public bool ConfirmPasswordAccessSytem(string numberText) {
        string numText = "";
        
        foreach (int n in _randomNumbers){
            numText += n.ToString();
        }

        if (numText == numberText){
            GameController.instance.UnlockDoor("Hall to Blue Room");
            Item item = GameController.instance.GetItem("Paper");
            Inventory.instance.Remove(item);
            return true;
        }
        else 
            return false;
    }

    public void TimeMachineConnections() {
        _connectionsMade++;

        if (_connectionsMade == 6) {
            TimeMachineAssembly.SetActive(false);
            Item item = GameController.instance.GetItem("Time Machine");
            Inventory.instance.Add(item);
            GameController.instance.DestroyItem("Time Machine");
            TalksController.instance.TimeMachineFinished();
        }
    }
}
