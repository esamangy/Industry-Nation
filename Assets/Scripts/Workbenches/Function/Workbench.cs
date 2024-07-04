using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workbench : MonoBehaviour, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private WorkbenchRecipeSO[] workbenchRecipeSOArray;
    [SerializeField] private MediumShelf[] connectedShelves;
    private float workbenchProgress;
    private Coroutine progressCoroutine;
    private int progressMultiplier = 0;

    private void Start() {
        Initialize();
    }

    private void OnValidate() {
        SortRecipeArray();
    }

    private void Initialize(){
        for(int i = 0; i < connectedShelves.Length; i ++){
            connectedShelves[i].OnInteractAlt += MediumShelf_OnInteractAlt;
            connectedShelves[i].OnObjectTakenFromHere += MediumShelf_OnObjectTakenFromHere;
        }
        MediumShelf.OnAnyObjectPlacedHere += MediumShelf_OnAnyObjectPlaced;
    }

    private void MediumShelf_OnAnyObjectPlaced(object sender, EventArgs e) {
        if(connectedShelves.ToList().Contains(sender as MediumShelf))
        if(progressCoroutine != null){
            StopCoroutine(progressCoroutine);
            workbenchProgress = 0;
            StartCoroutine(InteractAlertnateHold());
        }
    }

    private void MediumShelf_OnObjectTakenFromHere(object sender, EventArgs e) {
        if(progressCoroutine != null){
            StopCoroutine(progressCoroutine);
            workbenchProgress = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0
                });
        }
    }

    private void MediumShelf_OnInteractAlt(object sender, BaseWorkbench.PlayerEventArgs e) {
        FactoryObjectSO[] inputs = GetInputFactoryObjectArray();
        if(HasRecipeWithInput(inputs.ToArray())){
            e.player.GetGameInput().OnInteractAlternateActionStopped += GameInput_OnInteractAlternateActionStopped;
            e.player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
            if(progressMultiplier == 0){
                progressCoroutine = StartCoroutine(InteractAlertnateHold());
            }
            progressMultiplier ++;
        }
    }

    private IEnumerator InteractAlertnateHold(){
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(GetInputFactoryObjectArray());
        if(workbenchRecipeSO == null){
            yield break;
        }
        while(workbenchProgress < workbenchRecipeSO.workbenchProgressMax){
            workbenchProgress += Time.deltaTime * progressMultiplier;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = workbenchProgress / workbenchRecipeSO.workbenchProgressMax
                });
            yield return null;
        }

        FactoryObjectSO outputFactoryObjectSO = GetOutputForInput(GetInputFactoryObjectArray());

        foreach (MediumShelf shelf in connectedShelves) {
            if(shelf.GetFactoryObject() == null){
                //the shelf is empty
                continue;
            }
            if(!workbenchRecipeSO.inputs.ToList().Contains(shelf.GetFactoryObject().GetFactoryObjectSO())){
                //the shelf contains a factory object not included in the current recipe
                continue;
            }
            shelf.GetFactoryObject().DestroySelf();
        }
        MediumShelf ShelfToSpawnOn = connectedShelves[0].HasFactoryObject() ? (connectedShelves[1].HasFactoryObject() ? connectedShelves[2] : connectedShelves[1]) : connectedShelves[0];
        FactoryObject.SpawnFactoryObject(outputFactoryObjectSO, ShelfToSpawnOn);
        workbenchProgress = 0;
    }

    public FactoryObjectSO[] GetInputFactoryObjectArray(){
        List<FactoryObjectSO> inputs = new List<FactoryObjectSO>();
        foreach (MediumShelf shelf in connectedShelves) {
            if(shelf.HasFactoryObject()){
                inputs.Add(shelf.GetFactoryObject().GetFactoryObjectSO());
            }
        }
        return inputs.ToArray();
    }

    private void GameInput_OnInteractAlternateActionStopped(object sender, EventArgs e) {
        if(progressMultiplier == 1){
            StopCoroutine(progressCoroutine);
        }
        GameInput gameInput = sender as GameInput;
        gameInput.OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
        gameInput.GetComponent<PlayerController>().OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
        progressMultiplier --;
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(!connectedShelves.ToList().Contains(e.selectedBench)){
            if(progressMultiplier == 1){
                StopCoroutine(progressCoroutine);
            }
            PlayerController playerController = sender as PlayerController;
            playerController.OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
            playerController.GetGameInput().OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
            progressMultiplier --;
        }
    }

    private bool HasRecipeWithInput(FactoryObjectSO[] inputFactoryObjectSOs) {
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(inputFactoryObjectSOs);
        return workbenchRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO[] inputFactoryObjectSOs){
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(inputFactoryObjectSOs);
        if(workbenchRecipeSO != null){
            return workbenchRecipeSO.output;
        } else {
            return null;
        }
    }

    private WorkbenchRecipeSO GetWorkbenchRecipeSOWithInput(params FactoryObjectSO[] inputFactoryObjectSOs){
        List<FactoryObjectSO> inputs = inputFactoryObjectSOs.ToList();
        foreach (WorkbenchRecipeSO workbenchRecipeSO in workbenchRecipeSOArray) {
            bool valid = true;
            foreach(FactoryObjectSO ingredient in workbenchRecipeSO.inputs){
                if(!inputs.Contains(ingredient)){
                    valid = false;
                }
            }
            if(valid){
                return workbenchRecipeSO;
            }
        }
        return null;
    }

    private void SortRecipeArray() {
        List<WorkbenchRecipeSO> sortedList = new List<WorkbenchRecipeSO>();
        List<WorkbenchRecipeSO> unsortedList = workbenchRecipeSOArray.ToList();
        for(int i = 0; i < workbenchRecipeSOArray.Length; i ++){
            WorkbenchRecipeSO recipeWithMostIngredients = unsortedList[0];
            for(int j = 1; j < unsortedList.Count; j ++){
                if(unsortedList[j].inputs.Length > recipeWithMostIngredients.inputs.Length){
                    recipeWithMostIngredients = unsortedList[j];
                }
            }
            sortedList.Add(recipeWithMostIngredients);
            unsortedList.Remove(recipeWithMostIngredients);
        }
        workbenchRecipeSOArray = sortedList.ToArray();
    }
}
