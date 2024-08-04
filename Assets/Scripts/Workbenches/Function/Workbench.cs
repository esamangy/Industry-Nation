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
        BaseWorkbench.OnAnyObjectPlacedHere += MediumShelf_OnAnyObjectPlaced;
    }

    private void MediumShelf_OnAnyObjectPlaced(object sender, EventArgs e) {
        if(connectedShelves.ToList().Contains(sender as MediumShelf))
        if(progressCoroutine != null){
            StopCoroutine(progressCoroutine);
            workbenchProgress = 0;
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
        FactoryObjectSO outputFactoryObjectSO = workbenchRecipeSO.output;
        Dictionary<FactoryObjectSO, int> recipePieces = GetRecipePieces(workbenchRecipeSO);

        foreach (MediumShelf shelf in connectedShelves) {
            if(shelf.GetFactoryObject() == null){
                //the shelf is empty
                continue;
            }
            FactoryObjectSO shelfObjectSO = shelf.GetFactoryObject().GetFactoryObjectSO();
            if(recipePieces.ContainsKey(shelfObjectSO)){
                if(recipePieces[shelfObjectSO] > 0){
                    recipePieces[shelfObjectSO] --;
                    shelf.GetFactoryObject().DestroySelf();
                }
            }
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
    /// <summary>
    /// returns the first valid recipe in the recipes array. This works assuming the list is sorted
    /// </summary>
    /// <param name="inputFactoryObjectSOs">An array contained the inputs</param>
    /// <returns></returns>
    private WorkbenchRecipeSO GetWorkbenchRecipeSOWithInput(params FactoryObjectSO[] inputFactoryObjectSOs){
        for(int i = 0; i < workbenchRecipeSOArray.Length; i ++){
            WorkbenchRecipeSO recipeToCheck = workbenchRecipeSOArray[i];

            if(recipeToCheck.inputs.Length != inputFactoryObjectSOs.Length){
                // check if the recipe has the same number of pieces, move to next recipe if not
                continue;
            }
            Dictionary<FactoryObjectSO, int> inputPieces = new Dictionary<FactoryObjectSO, int>();
            foreach (FactoryObjectSO item in inputFactoryObjectSOs) {
                if(inputPieces.ContainsKey(item)){
                    inputPieces[item] ++;
                } else {
                    inputPieces.Add(item, 1);
                }
            }
            Dictionary<FactoryObjectSO, int> recipePices = GetRecipePieces(recipeToCheck);
            bool inputMatchesRecipe = true;
            foreach (KeyValuePair<FactoryObjectSO, int> inputPiece in inputPieces) {
                if(recipePices.ContainsKey(inputPiece.Key)){
                    // the delivered object has at least one of the same ingredient being checked
                    if(recipePices[inputPiece.Key] == inputPiece.Value){
                        //the delivered objetc has the same number of the piece being checked
                    } else {
                        //the delivered object has a different number of this piece and there is no point in continuing to check order
                        inputMatchesRecipe = false;
                        break;
                    }
                } else {
                    //the delivered object does not have this piece and there is no point in continuing to check order
                    inputMatchesRecipe = false;
                    break;
                }
            }
            if(inputMatchesRecipe){
                //player deliered a correct order
                return recipeToCheck;
            }
        }
        return null;
        // foreach (WorkbenchRecipeSO workbenchRecipeSO in workbenchRecipeSOArray) {
        //     bool valid = true;
        //     foreach(FactoryObjectSO ingredient in workbenchRecipeSO.inputs){
        //         if(!inputs.Contains(ingredient)){
        //             valid = false;
        //         }
        //     }
        //     if(valid){
        //         return workbenchRecipeSO;
        //     }
        // }
        // return null;
    }

    private Dictionary<FactoryObjectSO, int> GetRecipePieces(WorkbenchRecipeSO receipe){
        Dictionary<FactoryObjectSO, int> recipePices = new Dictionary<FactoryObjectSO, int>();
        foreach (FactoryObjectSO item in receipe.inputs) {
            if(recipePices.ContainsKey(item)){
                recipePices[item] ++;
            } else {
                recipePices.Add(item, 1);
            }
        }
        return recipePices;
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
