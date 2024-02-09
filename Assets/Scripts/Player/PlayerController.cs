using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance{get; private set;}
    //Events
    public event EventHandler<OnSelectedShelfChangedEventArgs> OnSelectedShelfChanged;
    public class OnSelectedShelfChangedEventArgs : EventArgs{
        public MediumShelf selectedShelf;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    private float playerHeight = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask interactLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDir;
    public MediumShelf selectedShelf;

    private void Awake() {
        if(Instance != null){
            throw new Exception("There is more than one player instance");
        }
        Instance = this;
    }

    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update(){
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .3f;
        
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if(!canMove){
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove){
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(moveDir.x, 0, 0);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if(canMove){
                    moveDir = moveDirZ;
                }
            }
        }

        if(canMove){
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir != Vector3.zero){
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position + (Vector3.up * .1f), lastInteractDir, out RaycastHit hit, interactDistance, interactLayerMask)){
            if(hit.transform.TryGetComponent(out MediumShelf mediumShelf)){
                if(mediumShelf != selectedShelf){
                    SetSelectedShelf(mediumShelf);
                }
            }else {
                SetSelectedShelf(null);
            }
        } else {
            SetSelectedShelf(null);
        }
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(selectedShelf != null){
            selectedShelf.Interact();
        }
    }
    public bool IsWalking(){
        return isWalking;
    }

    private void SetSelectedShelf(MediumShelf selectedShelf){
        this.selectedShelf = selectedShelf;
        OnSelectedShelfChanged?.Invoke(this, new OnSelectedShelfChangedEventArgs{
                        selectedShelf = selectedShelf
                    });
    }
}
