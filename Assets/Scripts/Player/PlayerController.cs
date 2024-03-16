using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IFactoryObjectParent {
    public static PlayerController Instance{get; private set;}
    //Events
    public event EventHandler<OnSelectedShelfChangedEventArgs> OnSelectedShelfChanged;
    public class OnSelectedShelfChangedEventArgs : EventArgs{
        public BaseWorkbench selectedBench;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    private float playerHeight = 1.5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private Transform factoryObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseWorkbench selectedBench;
    private FactoryObject factoryObject;

    private void Awake() {
        if(Instance != null){
            throw new Exception("There is more than one player instance");
        }
        Instance = this;
    }

    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
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
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove){
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(moveDir.x, 0, 0);
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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
            if(hit.transform.TryGetComponent(out BaseWorkbench baseWorkbench)){
                if(baseWorkbench != selectedBench){
                    SetSelectedBench(baseWorkbench);
                }
            }else {
                SetSelectedBench(null);
            }
        } else {
            SetSelectedBench(null);
        }
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(selectedBench != null){
            selectedBench.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if(selectedBench != null){
            selectedBench.InteractAlternate(this);
        }
    }
    public bool IsWalking(){
        return isWalking;
    }

    private void SetSelectedBench(BaseWorkbench baseWorkbench){
        selectedBench = baseWorkbench;
        OnSelectedShelfChanged?.Invoke(this, new OnSelectedShelfChangedEventArgs{
            selectedBench = baseWorkbench
        });
    }

    public Transform GetFactoryObjectFollowTransform(){
        return factoryObjectHoldPoint;
    }

    public void SetFactoryObject(FactoryObject factoryObject){
        this.factoryObject = factoryObject;
        if(factoryObject.TryGetBox(out BoxFactoryObject boxFactoryObject)){
            boxFactoryObject.TryOpen();
        }
    }

    public FactoryObject GetFactoryObject(){
        return factoryObject;
    }

    public void ClearFactoryObject(){
        factoryObject = null;
    }

    public bool HasFactoryObject(){
        return factoryObject != null;
    }
}
