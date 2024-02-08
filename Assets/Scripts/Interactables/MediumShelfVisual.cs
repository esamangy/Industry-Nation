using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelfVisual : MonoBehaviour {

    [SerializeField] private MediumShelf mediumShelf;
    [SerializeField] private GameObject SelectedShelfVisual;
    private void Start() {
        PlayerController.Instance.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedShelf == mediumShelf){
            Show();
        } else {
            Hide();
        }
    }

    private void Show(){
        SelectedShelfVisual.SetActive(true);
    }

    private void Hide(){
        SelectedShelfVisual.SetActive(false);
    }
}
