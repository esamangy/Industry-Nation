using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnerPallet : BaseWorkbench {
    public event EventHandler OnBoxSpawned;
    public event EventHandler OnBoxRemoved;
    [SerializeField] private FactoryObjectSO boxFactoryObjectSO;
    [Range(0,10)]
    [SerializeField] private float spawnPlateTimerMax = 4f;
    private float spawnBoxTimer;
    private int boxesSpawned;
    [SerializeField] private int maxSpawnedPlates = 4;
    private void Update() {
        spawnBoxTimer += Time.deltaTime;
        if(spawnBoxTimer > spawnPlateTimerMax){
            spawnBoxTimer = 0f;

            if(GameManager.Instance.IsGamePlaying() &&  boxesSpawned < maxSpawnedPlates){
                boxesSpawned ++;
                OnBoxSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(PlayerController player) {
        if(!player.HasFactoryObject()){
            //player is empty handed
            if(boxesSpawned > 0){
                boxesSpawned --;
                FactoryObject.SpawnFactoryObject(boxFactoryObjectSO, player);

                OnBoxRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
