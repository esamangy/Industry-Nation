using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour {

    [SerializeField] private BaseWorkbench baseWorkbench;
    [SerializeField] private GameObject[] VisualGameobjectArray;
    private List<PlayerController> playersSelecting;
    private void Awake() {
        playersSelecting = new List<PlayerController>();
    }
    private void Start() {
        PlayersManager.Instance.OnPlayerListChanged += PlayerController_OnPlayerListChanged;
        RegisterPlayers();
    }
    private void RegisterPlayers() {
        List<PlayerController> players = PlayersManager.Instance.GetPlayers();
        foreach (PlayerController player in players) {
            player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
        }
    }
    private void PlayerController_OnPlayerListChanged(object sender, EventArgs e) {
        RegisterPlayers();
    }
    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        PlayerController sendingPlayer = sender as PlayerController;
        if(e.selectedBench == baseWorkbench){
            //the selected bench is this bench
            if(!playersSelecting.Contains(sendingPlayer)){
                playersSelecting.Add(sendingPlayer);
            }
            Show();
        } else {
            // the selected bench is another bench
            if(playersSelecting.Contains(sendingPlayer)){
                playersSelecting.Remove(sendingPlayer);
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
    }

    private void Hide(){
        foreach (GameObject visualGameObject in VisualGameobjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
