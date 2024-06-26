using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IFactoryObjectParent {
    //Events
    public static event EventHandler OnAnyPlayerPaused;
    public event EventHandler OnGrabbedSomething;
    public event EventHandler<OnSelectedShelfChangedEventArgs> OnSelectedShelfChanged;
    public class OnSelectedShelfChangedEventArgs : EventArgs{
        public BaseWorkbench selectedBench;
    }

    [SerializeField] private float moveForce = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private Transform factoryObjectHoldPoint;
    private Rigidbody m_rigidbody;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseWorkbench selectedBench;
    private FactoryObject factoryObject;
    private void Awake() {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnPausedAction += GameInput_OnPausedAction;
    }

    private void Update(){
        HandleMovement();
        HandleInteractions();
    }
    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 input = new Vector3(inputVector.x, 0, inputVector.y);

        if(input.magnitude > .1f){
            isWalking = true;
        } else {
            isWalking = false;
        }

        m_rigidbody.AddForce(input * moveForce);

        transform.forward = Vector3.Slerp(transform.forward, input, Time.deltaTime * rotateSpeed);
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
        if(!GameManager.Instance.IsGamePlaying()){
            return;
        }
        if(selectedBench != null){
            selectedBench.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()){
            return;
        }
        if(selectedBench != null){
            selectedBench.InteractAlternate(this);
        }
    }
    private void GameInput_OnPausedAction(object sender, EventArgs e) {
        OnAnyPlayerPaused?.Invoke(this, EventArgs.Empty);
    }
    public bool IsWalking(){
        return isWalking;
    }

    public GameInput GetGameInput(){
        return gameInput;
    }

    private void SetSelectedBench(BaseWorkbench baseWorkbench){
        selectedBench = baseWorkbench;
        OnSelectedShelfChanged?.Invoke(this, new OnSelectedShelfChangedEventArgs{
            selectedBench = baseWorkbench,
        });
    }

    public Transform GetFactoryObjectFollowTransform(){
        return factoryObjectHoldPoint;
    }

    public void SetFactoryObject(FactoryObject factoryObject){
        this.factoryObject = factoryObject;
        if(factoryObject != null){
            OnGrabbedSomething?.Invoke(this, EventArgs.Empty);
        }
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
