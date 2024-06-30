using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuPlayers : MonoBehaviour {
    private struct PositionData{
        public Transform location{get; private set;}
        public bool taken;
        public PlayerController player;
        public void SetPosition(Transform position){
            location = position; 
        }
        public void SetData(bool taken, PlayerController player){
            if(this.taken && player == null){
                Destroy(this.player.gameObject);
            } else {
                this.taken = taken;
                this.player = player;
                if(this.player != null){
                    this.player.transform.position = location.position;
                    this.player.transform.rotation = location.rotation;
                }
            }
        }
    }
    [SerializeField] private Transform[] locations;
    private PositionData[] positionDatas;
    private void Start() {
        PlayersManager.Instance.OnPlayerListChanged += PlayersManager_OnPlayerListChanged;
        positionDatas = new PositionData[locations.Length];
        InitializePositions();
    }

    private void InitializePositions() {
        for(int i = 0; i < positionDatas.Length; i ++){
            positionDatas[i].SetPosition(locations[i]);
            positionDatas[i].SetData(false, null);
        }
    }

    private void PlayersManager_OnPlayerListChanged(object sender, EventArgs e) {
        List<PlayerController> players = PlayersManager.Instance.GetPlayers();
        List<PlayerController> spawnedPlayers = GetSpawnedPlayers();

        if(players.Count > spawnedPlayers.Count) {
            // a player has joined
            PlayerController joinPlayer = null;
            foreach (PlayerController player in players) {
                if(!spawnedPlayers.Contains(player)){
                    joinPlayer = player;
                }
            }
            List<int> indexes = new List<int>();
            for(int i = 0; i < positionDatas.Length; i ++){
                if(!positionDatas[i].taken){
                    indexes.Add(i);
                }
            }
            int index = UnityEngine.Random.Range(0, indexes.Count);
            positionDatas[indexes[index]].SetData(true, joinPlayer);

        } else if(players.Count < spawnedPlayers.Count){
            // a player has left
            PlayerController leftPlayer = null;
            //determine which player left
            foreach (PlayerController player in spawnedPlayers) {
                if(!players.Contains(player)){
                    leftPlayer = player;
                }
            }
            if(leftPlayer == null){
                Debug.LogWarning("A player has left but was not registered before");
                return;
            }
            for(int i = 0; i < positionDatas.Length; i ++){
                if(positionDatas[i].player == leftPlayer){
                    positionDatas[i].SetData(false, null);
                }
            }
        } else {
            // player count stayed the same. this shouldnt happen right now
            Debug.LogWarning("Player count stayed the same. There is currently no handling for this situation");
        }
    }

    private List<PlayerController> GetSpawnedPlayers() {
        List<PlayerController> toReturn = new List<PlayerController>();
        foreach(PositionData positionData in positionDatas){
            if(positionData.player != null){
                toReturn.Add(positionData.player);
            }
        }

        return toReturn;
    }
}
