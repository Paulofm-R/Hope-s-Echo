using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;

    public float speed = 3.0f;
    public bool hasInteracted = false; // To prevent the player from picking up duplicate items
    public bool hasPlayerMove = true;

    public AudioSource footstepsSpound;

    private Vector3 _target; // position the player will move
    private Vector3 _mouseClick; // area where the mouse clicked
    private Vector3 _lTemp;
    private Animator _animator;
    private bool _hasPaper = false; // Prevent you from receiving the same paper twice

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start() {
        _target = transform.position;
        _animator = GetComponent<Animator>();
        _lTemp = transform.localScale;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && hasPlayerMove) {
            _mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _target = _mouseClick;
            _target.z = transform.position.z;
            _target.y = transform.position.y;

            // Will disable a gameobject when the player moves
            PuzzleController.instance.DeactivateIfActivated();
        }

        if (transform.position.x != _target.x) {
            PlayerMoved();
            footstepsSpound.enabled = true;
        } else {
            _animator.SetBool("PlayerWalk", false);
            footstepsSpound.enabled = false;
        }
    }

    public void StoppedPlayer() {
        _target = transform.position;
        _animator.SetBool("PlayerWalk", false);
        footstepsSpound.enabled = false;
    }

    private void PlayerMoved(){
        _animator.SetBool("PlayerWalk", true);
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);

        if ((transform.position.x < _target.x && _lTemp.x > 0) || (transform.position.x > _target.x && _lTemp.x < 0)) {
            _lTemp.x *= -1;
            transform.localScale = _lTemp;
        }

        // Checks whether the click occurred in the object area
        Collider2D hitCollider = Physics2D.OverlapPoint(_mouseClick);

        if (hitCollider) {
            GameObject Collider = hitCollider.gameObject;
            float distance = Vector3.Distance(transform.position, Collider.transform.position);

            if (Collider.CompareTag("ChangeDivision")  && distance < 0.6f){
                GameController.instance.ChangeDivision(Collider.name, gameObject);
                StoppedPlayer();
            } else if (Collider.CompareTag("ChangeDivisionClosed")  && distance < 0.6f){
                StoppedPlayer();
                StartCoroutine(TalksController.instance.Doors(Collider));
            } else if (Collider.CompareTag("NPC")  && distance < 1.2f) {
                bool hasSeenDoor = TalksController.instance.NPC();
                if (hasSeenDoor) { 
                    PuzzleController.instance.NPCInteract(_hasPaper);
                    _hasPaper = true;
                }
                StoppedPlayer();
            } else if (Collider.CompareTag("Item") && distance < 1f) {
                GameController.instance.ItemPickup(Collider);
                StoppedPlayer();
            } else if (Collider.CompareTag("Puzzel") && distance < 0.95f && !hasInteracted) {
                hasInteracted = true;
                StartCoroutine(PuzzleController.instance.Interact(Collider));
                StoppedPlayer();
            } else if (Collider.CompareTag("AccessSystem") && distance < 1f) {
                PuzzleController.instance.KeypadAccessSytem.SetActive(true);
                StoppedPlayer();
            } else if (Collider.CompareTag("WhiteBoard") && distance < 2f){
                PuzzleController.instance.WhiteBoard.SetActive(true);
                StoppedPlayer();
            } else if (Collider.CompareTag("WorkBench") && distance < 4f){
                hasInteracted = true;
                StartCoroutine(PuzzleController.instance.Interact(Collider));
            } else if (Collider.CompareTag("Bookshelfs") && distance < 0.85f) {
                TalksController.instance.Bookshelfs(false);
                StoppedPlayer();
            }
        }
    }

}
