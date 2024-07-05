using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : BaseWorkbench, IHasProgress {
    [SerializeField] private float spawnOrderTimerMin = 3f;
    [SerializeField] private float spawnOrderTimerMax = 10f;
    [SerializeField] private float ringingTimerMax = 6f;
    [SerializeField] private float answerPhoneProgressMax = 2f;
#if UNITY_EDITOR
    [SerializeField] private bool DebugAutoAnswerPhone = false;
#endif
    private bool isRinging = false;
    private bool isAnswering = false;
    private float spawnOrderTimer = 2f;
    private float ringingTimer;
    private float answerPhoneProgress;
    private Coroutine progressCoroutine;
    private int progressMultiplier = 0;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnRingingTimerChanged;
    public override void InteractAlternate(PlayerController player){
        base.InteractAlternate(player);
        if(isRinging){
            player.GetGameInput().OnInteractAlternateActionStopped += GameInput_OnInteractAlternateActionStopped;
            player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
            if(progressMultiplier == 0){
                progressCoroutine = StartCoroutine(InteractAlertnateHold());
            }
            progressMultiplier ++;
            isAnswering = true;
        }
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedBench != this){
            if(progressMultiplier == 1){
                isAnswering = false;
                StopCoroutine(progressCoroutine);
            }
            PlayerController playerController = sender as PlayerController;
            playerController.OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
            playerController.GetGameInput().OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
            progressMultiplier --;
        }
    }

    private void GameInput_OnInteractAlternateActionStopped(object sender, System.EventArgs e) {
        if(progressMultiplier == 1){
            isAnswering = false;
            StopCoroutine(progressCoroutine);
        }
        GameInput gameInput = sender as GameInput;
        gameInput.OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
        gameInput.GetComponent<PlayerController>().OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
        progressMultiplier --;
    }

    private void Update() {
        if(!GameManager.Instance.IsGamePlaying()){
            return;
        }
        if(isRinging && !isAnswering){
            ringingTimer -= Time.deltaTime;
            OnRingingTimerChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                progressNormalized =  1 - (ringingTimer / ringingTimerMax)
            });
            if(ringingTimer <= 0f){
                isRinging = false;
                spawnOrderTimer = spawnOrderTimerMax;
                OnRingingTimerChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized =  1
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0
                });
            }
        } else {
            spawnOrderTimer -= Time.deltaTime;
            if(spawnOrderTimer <= 0f){
                spawnOrderTimer = UnityEngine.Random.Range(spawnOrderTimerMin, spawnOrderTimerMax);

                if(!DeliveryManager.Instance.IsWaitingListFull()){
#if UNITY_EDITOR
                    if(DebugAutoAnswerPhone){
                        DeliveryManager.Instance.AddOrderToWaitList();
                        return;
                    }
#endif
                    OnRingingTimerChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0
                    });
                    isRinging = true;
                    ringingTimer = ringingTimerMax;
                }
            }
        }
    }

    public override void SetFactoryObject(FactoryObject factoryObject){
        return;
    }
    
    private IEnumerator InteractAlertnateHold(){
        while(answerPhoneProgress < answerPhoneProgressMax){
            answerPhoneProgress += Time.deltaTime * progressMultiplier;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = answerPhoneProgress / answerPhoneProgressMax
                });
            yield return null;
        }

        DeliveryManager.Instance.AddOrderToWaitList();
        answerPhoneProgress = 0;
        isRinging = false;
        isAnswering = false;
        OnRingingTimerChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
            progressNormalized = 1
        });
    }

    public float GetRingingTimerMax(){
        return ringingTimerMax;
    }
}
