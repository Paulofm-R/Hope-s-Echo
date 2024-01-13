using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccessSytemController : MonoBehaviour {
    public TMP_InputField charHolder;

    void Start() {
        charHolder.interactable = false; // Disables interactivity and input in TMP_InputField
    }

    public void AddNumber(string number) {
        if (charHolder.text.Length < 6) {
            charHolder.text += number;
        }
    }

    public void Clear() {
        charHolder.text = "";
    }

    public void Enter() {
        if (PuzzleController.instance.ConfirmPasswordAccessSytem(charHolder.text)) 
            PuzzleController.instance.KeypadAccessSytem.SetActive(false);
        else {
            charHolder.text = "XXXX";
            StartCoroutine(ClearWrong());
        }
            
    }

    private IEnumerator ClearWrong() {

        yield return new WaitForSeconds(1);
        charHolder.text = "";
    }
}
