using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnerPalletVisual : MonoBehaviour {

    [SerializeField] private BoxSpawnerPallet boxSpawnerPallet;
    public List<GameObject> boxVisuals;

    private void Awake() {
        boxSpawnerPallet.OnBoxSpawned += BoxSpawnerPallet_OnBoxSpawned;
        boxSpawnerPallet.OnBoxRemoved += BoxSpawnerPallet_OnBoxRemoved;
    }

    private void BoxSpawnerPallet_OnBoxSpawned(object sender, EventArgs e) {
        foreach(GameObject box in boxVisuals){
            if(!box.activeSelf){
                box.SetActive(true);
                break;
            }
        }
    }

    private void BoxSpawnerPallet_OnBoxRemoved(object sender, EventArgs e) {
        for(int i = boxVisuals.Count - 1; i >= 0; i --){
            if(boxVisuals[i].activeSelf){
                boxVisuals[i].SetActive(false);
                break;
            }
        }
    }
}
