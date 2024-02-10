using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour {

    [SerializeField] private BaseWorkbench baseWorkbench;
    [SerializeField] private GameObject[] VisualGameobjectArray;
    private void Start() {
        PlayerController.Instance.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedBench == baseWorkbench){
            Show();
        } else {
            Hide();
        }
    }

    private void Show(){
        foreach (GameObject visualGameObject in VisualGameobjectArray) {
            visualGameObject.SetActive(true);
        }
        
    }

    private void Hide(){
        foreach (GameObject visualGameObject in VisualGameobjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
