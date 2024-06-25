using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour {

    [SerializeField] private BaseWorkbench baseWorkbench;
    [SerializeField] private GameObject[] VisualGameobjectArray;
    private List<PlayerController> playersSelecting;
    private bool isShown = false;
    private void Awake() {
        playersSelecting = new List<PlayerController>();
    }
    private void Start() {
        PlayerController.OnAnySelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedBench == baseWorkbench){
            //the selected bench is this bench
            if(!playersSelecting.Contains(e.playerSelecting)){
                playersSelecting.Add(e.playerSelecting);
            }
            Show();
        } else {
            // the selected bench is another bench
            if(playersSelecting.Contains(e.playerSelecting)){
                playersSelecting.Remove(e.playerSelecting);
            }
            if(playersSelecting.Count == 0){
                Hide();
            }
        }
    }

    private void Show(){
        foreach (GameObject visualGameObject in VisualGameobjectArray) {
            visualGameObject.SetActive(true);
        }
        isShown = true;
    }

    private void Hide(){
        foreach (GameObject visualGameObject in VisualGameobjectArray) {
            visualGameObject.SetActive(false);
        }
        isShown = false;
    }
}
