using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalksController : MonoBehaviour {
    public static TalksController instance;

    public GameObject cat;
    private AudioSource _catAudio;

    private bool _hasSeenDoor = false;
    private bool _hasTalkCat = false;
    private bool _hasPaper = false;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start() {
        _catAudio = cat.GetComponent<AudioSource>();
    }

    public IEnumerator TalkCat(bool _hasCatEaten) {
        _catAudio.enabled = true;
        PlayerController.instance.hasPlayerMove = false;

        if (!_hasCatEaten) {
            GameController.instance.ShowDialogues("Hi cat!", 1f);
            yield return new WaitForSeconds(1);
            GameController.instance.ShowDialogues("Why are you biting the green room card?", 3f);
            yield return new WaitForSeconds(3);
            GameController.instance.ShowDialogues("Are you hungry?", 2f);
            yield return new WaitForSeconds(2);
            GameController.instance.ShowDialogues("I should get him something to eat", 2f);
            yield return new WaitForSeconds(2);
            _hasTalkCat = true;
        } else {
            GameController.instance.ShowDialogues("Are you full yet?", 1.5f);
            yield return new WaitForSeconds(2);
        }

        PlayerController.instance.hasPlayerMove = true;
        _catAudio.enabled = false;
    }

    public IEnumerator Doors(GameObject door) {
        PlayerController.instance.hasPlayerMove = false;

        GameController.instance.ShowDialogues("It's closed", 2f);
        yield return new WaitForSeconds(2);
        if (door.name == "Door2_Hall") {
            if (!_hasTalkCat){
                GameController.instance.ShowDialogues("I have to see where I left the card for this room", 3f);
                yield return new WaitForSeconds(3);
                GameController.instance.ShowDialogues("It seemed to me that I saw him with the cat. I will confirm", 4f);
                yield return new WaitForSeconds(4);
            } else {
                GameController.instance.ShowDialogues("I have to find food to give the cat to get the card back", 4f);
                yield return new WaitForSeconds(4);
            }
            
        }
        if (door.name == "Door3_Hall") {
            if (!_hasPaper){
                GameController.instance.ShowDialogues("I don't remember the password", 2f);
                yield return new WaitForSeconds(2);
                GameController.instance.ShowDialogues("I'll talk to my colleague to see if he remembers", 3f);
                yield return new WaitForSeconds(3);
                _hasSeenDoor = true;
            } else {
                GameController.instance.ShowDialogues("I have to enter the code first", 2f);
            }
            
        }

        PlayerController.instance.hasPlayerMove = true;
    }

    public bool NPC() {
        if (!_hasSeenDoor) {
            GameController.instance.ShowDialogues("Hi!", 2f);
            return false;
        } else {
            GameController.instance.ShowDialogues("Here is the paper with the password", 3f);
            _hasPaper = true;
            return true;
        }
    }

    public void Workbench() {
        GameController.instance.ShowDialogues("I already have 4 pieces. Now I need to go to the blue mounts room", 5f);
    }

    public void Bookshelfs(bool ItemsInside){
        if (!ItemsInside)
            GameController.instance.ShowDialogues("Nothing here", 2f);
        else 
            GameController.instance.ShowDialogues("It's locked. I need to find the keys", 3f);
    }
    
    public void CompressorFinished(){
        GameController.instance.ShowDialogues("It is mounted! Now I need to put this in the time machine.", 4f);
    }

    public void TimeMachineFinished(){
        GameController.instance.ShowDialogues("Finally the time machine is made! Now it's time to try it out!", 4f);
    }

    public IEnumerator FinishDemoGame(){
        PlayerController.instance.hasPlayerMove = false;

        yield return new WaitForSeconds(2);
        GameController.instance.ShowDialogues("Oh no... The time machine ran out of power...", 4f);
        yield return new WaitForSeconds(4);
        GameController.instance.ShowDialogues("I'll have to see if there's a power source here..", 3f);
        yield return new WaitForSeconds(3);

        GameController.instance.FinishGame();
    }
}
