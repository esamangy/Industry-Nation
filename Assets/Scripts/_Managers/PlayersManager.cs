using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayersManager : MonoBehaviour {
    [SerializeField] private GameObject canvas;
    public static PlayersManager Instance; 
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> players;
    public event EventHandler OnPlayerListChanged;
    private void Awake() {
        Instance = this;
        players = new List<PlayerInput>();

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += PlayerInputManager_OnPlayerJoined;
        playerInputManager.onPlayerLeft += PlayerInputManager_OnPlayerLeft;
    }

    private void PlayerInputManager_OnPlayerJoined(PlayerInput input) {
        players.Add(input);
        OnPlayerListChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInputManager_OnPlayerLeft(PlayerInput input) {
        if(players.Contains(input)){
            players.Remove(input);
            OnPlayerListChanged?.Invoke(this, EventArgs.Empty);
        } else {
            Debug.LogWarning("A player that was never registered has left");
        }
    }

    public List<PlayerController> GetPlayers() {
        List<PlayerController> toReturn = new List<PlayerController>();
        foreach (PlayerInput playerInput in players) {
            toReturn.Add(playerInput.GetComponent<PlayerController>());
        }
        return toReturn;
    }

    public void SetControllingPlayer(PlayerController player){
        MultiplayerEventSystem eventSystem = player.GetComponent<MultiplayerEventSystem>();
        eventSystem.playerRoot = canvas;
        eventSystem.enabled = true;
        EventSystem.current = eventSystem;

    }

    public void RemoveControlFromPlayer(PlayerController player){
        MultiplayerEventSystem eventSystem = player.GetComponent<MultiplayerEventSystem>();
        player.GetComponent<MultiplayerEventSystem>().playerRoot = null;
        eventSystem.enabled = false;        
    }
}
