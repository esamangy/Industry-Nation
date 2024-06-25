using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance; 
    private PlayerInputManager playerInputManager;

    private void Awake() {
        Instance = this;
        
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += PlayerInputManager_OnPlayerJoined;
        playerInputManager.onPlayerLeft += PlayerInputManager_OnPlayerLeft;
    }

    private void PlayerInputManager_OnPlayerJoined(PlayerInput input) {
        print("Joined");
    }

    private void PlayerInputManager_OnPlayerLeft(PlayerInput input) {
        print("Left");
    }
}
